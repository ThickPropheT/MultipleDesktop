using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using System;
using System.Collections.Generic;
using static MultipleDesktop.Windows.Interop.Shell.NativeMethods;
using uIVirtualDesktop = MultipleDesktop.Windows.Interop.Shell.NativeMethods.IVirtualDesktop;

namespace MultipleDesktop.Windows.Interop.Shell
{
    public class WindowsShellAdapter : IWindowsDesktopAdapter
    {
        private readonly IConfigurationFactory _factory;

        private readonly IDesktopWallpaper _desktop;
        private readonly IVirtualDesktopManagerInternal _managerInternal;

        private readonly object _ioLock = new object();

        internal WindowsShellAdapter(IConfigurationFactory factory, IDesktopWallpaper desktop, IVirtualDesktopManagerInternal managerInternal)
        {
            _factory = factory;
            _desktop = desktop;
            _managerInternal = managerInternal;
        }

        public Guid LoadCurrentDesktopUuid()
        {
            return _managerInternal.GetCurrentDesktop().GetId();
        }

        public IEnumerable<Guid> LoadDesktopUuidList()
        {
            var uuids = new List<Guid>();

            var numberOfDesktops = _managerInternal.GetCount();

            IObjectArray desktops;
            _managerInternal.GetDesktops(out desktops);

            for (int i = 0; i < numberOfDesktops; i++)
            {
                object desktopResult;
                desktops.GetAt(i, typeof(uIVirtualDesktop).GUID, out desktopResult);

                uuids.Add(((uIVirtualDesktop)desktopResult).GetId());
            }

            return uuids;
        }

        public void SaveBackground(IBackground background)
        {
            lock (_ioLock)
            {
                var result = _desktop.SetWallpaper(null, background.Path);
                result = _desktop.SetPosition(background.Fit.ToPosition());
            }
        }

        public IBackground LoadBackground()
        {
            lock (_ioLock)
            {
                var path = new string(default(char), Constants.MaxPath);
                _desktop.GetWallpaper(null, ref path);

                DESKTOP_WALLPAPER_POSITION position;
                _desktop.GetPosition(out position);

                return _factory.BackgroundFrom(
                    path,
                    position.ToFit());
            }
        }
    }
}
