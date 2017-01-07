using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using MultipleDesktop.Windows.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using uIVirtualDesktop = MultipleDesktop.Mvc.Desktop.IVirtualDesktop;

namespace MultipleDesktop.Windows
{
    public class VirtualDesktopStateProvider : IVirtualDesktopState
    {
        private readonly IWindowsDesktop _windowsDesktop;
        private readonly IWindowsDesktopAdapter _adapter;
        private readonly IConfigurationFactory _desktopFactory;

        private readonly Timer _updateTimer;

        private List<uIVirtualDesktop> _allDesktops = new List<uIVirtualDesktop>();

        public IEnumerable<uIVirtualDesktop> AllDesktops =>
            GetAllDesktopsAtomic(
                _adapter.LoadDesktopUuidList()
                    .Count());

        public uIVirtualDesktop Current { get; private set; }

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        public VirtualDesktopStateProvider(IWindowsDesktop windowsDesktop, IWindowsDesktopAdapter adapter, IConfigurationFactory factory)
        {
            _windowsDesktop = windowsDesktop;
            _adapter = adapter;
            _desktopFactory = factory;

            _updateTimer = new Timer(Mvc.Constants.Default.Ui.UpdateRate.TotalMilliseconds)
            {
                AutoReset = false
            };

            _updateTimer.Elapsed += _updateTimer_Elapsed;

            LoadBackgroundList();
        }

        public void Load()
        {
            _updateTimer.Start();
        }

        private void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _updateTimer.Stop();

            try
            {
                UpdateFromWindows();
            }
            finally
            {
                _updateTimer.Start();
            }
        }

        private void UpdateFromWindows()
        {
            var currentBackground = _windowsDesktop.Background;
            _windowsDesktop.Update();

            var uuids = _adapter.LoadDesktopUuidList();

            var latestDesktops = GetAllDesktopsAtomic(uuids.Count());

            if (uuids.Count() != latestDesktops.Count)
            {
                OnPropertyChanging(nameof(AllDesktops));

                bool success;
                try
                {
                    LoadBackgroundList(latestDesktops, uuids);

                    SetAllDesktopsAtomic(latestDesktops);

                    success = true;
                }
                catch (Exception ex)
                {
                    OnPropertyChanging(new PropertyRollBackChangesEventArgs(nameof(AllDesktops), ex));

                    success = false;
                }

                if (success)
                    OnPropertyChanged(nameof(AllDesktops));
            }

            var latestCurrent = latestDesktops.FirstOrDefault(desktop => desktop.IsCurrent);

            if (!Equals(latestCurrent, Current))
            {
                Current = latestCurrent;

                OnPropertyChanged(nameof(Current));
            }
        }

        private void SetAllDesktopsAtomic(IList<uIVirtualDesktop> allDesktops)
        {
            lock (((ICollection)_allDesktops).SyncRoot)
            {
                _allDesktops = new List<uIVirtualDesktop>(allDesktops);
            }
        }

        private IList<uIVirtualDesktop> GetAllDesktopsAtomic(int numberOfUuids)
        {
            lock (((ICollection)_allDesktops).SyncRoot)
            {
                var capacity = _allDesktops.Count > numberOfUuids
                    ? _allDesktops.Count
                    : numberOfUuids;

                var allDesktops = new List<uIVirtualDesktop>(capacity);
                allDesktops.AddRange(_allDesktops);
                return allDesktops;
            }
        }

        private void OnPropertyChanging(System.ComponentModel.PropertyChangingEventArgs args)
        {
            PropertyChanging?.Invoke(this, args);
        }

        private void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Load

        private void LoadBackgroundList()
        {
            var uuids = _adapter.LoadDesktopUuidList();
            var numberOfUuids = uuids.Count();

            var allDesktops = GetAllDesktopsAtomic(numberOfUuids);

            LoadBackgroundList(allDesktops, uuids);

            SetAllDesktopsAtomic(allDesktops);
        }

        private void LoadBackgroundList(IList<uIVirtualDesktop> list, IEnumerable<Guid> uuids)
        {
            uint index = 0;
            foreach (var guid in uuids)
            {
                var existingDesktop = list.FirstOrDefault(desktop => desktop.Guid.Equals(guid));

                if (existingDesktop != null)
                    list.Remove(existingDesktop);

                list.Add(_desktopFactory.DesktopFrom(guid, index, _windowsDesktop));

                index++;
            }
        }

        #endregion Load
    }
}
