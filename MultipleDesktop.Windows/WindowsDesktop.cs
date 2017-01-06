using MultipleDesktop.Mvc.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using wRegistry = Microsoft.Win32.Registry;

namespace MultipleDesktop.Windows
{
    public class WindowsDesktop : IWindowsDesktop
    {
        private IBackground _background;

        private readonly object _ioLock = new object();

        public IBackground Background
        {
            get { return _background; }
            set
            {
                _background = value;
                SaveBackground(value, _ioLock);
            }
        }

        public Guid Guid { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowsDesktop()
        {
            _background = LoadBackground(_ioLock);
        }

        public void Update()
        {
            var latestGuid = LoadCurrentDesktopUuid();

            if (!Equals(Guid, latestGuid))
            {
                Guid = latestGuid;

                OnPropertyChanged(nameof(Guid));
            }

            var latestBackground = LoadBackground(_ioLock);

            if (!Equals(latestBackground, _background))
            {
                _background = latestBackground;

                OnPropertyChanged(nameof(Background));
            }
        }

        private static IBackground LoadBackground(object loadLock)
        {
            const int maxPath = 260;
            var pathBuffer = new string(default(char), maxPath);

            lock (loadLock)
            {
                NativeMethods.SystemParametersInfo(
            NativeMethods.SPI_GETDESKWALLPAPER,
            pathBuffer.Length,
            pathBuffer,
            0);
            }

            var path = pathBuffer.Substring(0, pathBuffer.IndexOf(default(char)));

            // TODO load fit
            return new Background(path, default(Fit));
        }

        private static void SaveBackground(IBackground background, object saveLock)
        {
            lock (saveLock)
            {
                using (var key = wRegistry.CurrentUser.OpenSubKey(
                    Registry.Desktop.SubKey, true))
                {
                    key.SetValue(
                        Registry.Desktop.TileWallpaper.Key,
                        Registry.Desktop.TileWallpaper.DataFromFit(background.Fit));

                    key.SetValue(
                        Registry.Desktop.WallpaperStyle.Key,
                        Registry.Desktop.TileWallpaper.DataFromFit(background.Fit));
                }

                NativeMethods.SystemParametersInfo(
                    NativeMethods.SPI_SETDESKWALLPAPER,
                    0,
                    background.Path,
                    NativeMethods.SPIF_UPDATEINIFILE
                    | NativeMethods.SPIF_SENDCHANGE);
            }
        }

        public Guid LoadCurrentDesktopUuid()
        {
            var sessionId = Process.GetCurrentProcess().SessionId;

            using (var subKey = wRegistry.CurrentUser.OpenSubKey(
                Registry.Desktop.CurrentVirtualDesktop.SubKeyFor(sessionId)))
            {
                // TODO CurrentVirtualDesktop value is null from Start until
                // TODO a desktop is switched
                var current = subKey.GetValue(
                    Registry.Desktop.CurrentVirtualDesktop.Value);

                var guid = new Guid((byte[])current);

                return guid;
            }
        }

        public IEnumerable<Guid> LoadDesktopUuidList()
        {
            using (var subKey = wRegistry.CurrentUser.OpenSubKey(
                Registry.Desktop.VirtualDesktopIDs.SubKey))
            {
                var guidsValue = (byte[])subKey.GetValue(
                    Registry.Desktop.VirtualDesktopIDs.Value);

                var guids = new List<Guid>();

                var index = 0;
                var length = Registry.Desktop.UuidLength;
                while (index + length <= guidsValue.Length)
                {
                    var buffer = new byte[length];
                    Array.Copy(guidsValue, index, buffer, 0, length);

                    index += length;

                    guids.Add(new Guid(buffer));
                }

                return guids;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
