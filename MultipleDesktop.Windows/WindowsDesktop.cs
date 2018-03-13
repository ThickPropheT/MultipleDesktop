using MultipleDesktop.Mvc.Desktop;
using MultipleDesktop.Windows.Interop;
using System;
using System.ComponentModel;

namespace MultipleDesktop.Windows
{
    public class WindowsDesktop : IWindowsDesktop
    {
        private readonly IWindowsDesktopAdapter _adapter;

        private IBackground _background;

        public IBackground Background
        {
            get { return _background; }
            set
            {
                _background = value;
                _adapter.SaveBackground(value);
            }
        }

        public Guid Guid { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowsDesktop(IWindowsDesktopAdapter adapter)
        {
            _adapter = adapter;

            UpdateFromAdapter();
        }

        public void Update()
            => UpdateFromAdapter();

        private void UpdateFromAdapter()
        {
            var latestGuid = _adapter.LoadCurrentDesktopUuid();

            if (!Equals(Guid, latestGuid))
            {
                Guid = latestGuid;

                OnPropertyChanged(nameof(Guid));
            }

            var latestBackground = _adapter.LoadBackground();

            if (!Equals(latestBackground, _background))
            {
                _background = latestBackground;

                OnPropertyChanged(nameof(Background));
            }
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
