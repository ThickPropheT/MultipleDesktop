using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MultipleDesktop.Mvc.Desktop
{
    public class VirtualDesktop : IVirtualDesktop
    {
        private readonly ISystemDesktop _systemDesktop;

        private IBackground _background;
        private bool _isCurrent;

        public IBackground Background
        {
            get { return _background; }
            set
            {
                if (value == null)
                    return;

                _background = value;

                if (IsCurrent)
                    _systemDesktop.Background = value;
            }
        }

        public Guid Guid { get; }

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                if (Equals(_isCurrent, value))
                    return;

                _isCurrent = value;

                if (value)
                    _systemDesktop.Background = _background;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public VirtualDesktop(Guid guid, ISystemDesktop systemDesktop)
        {
            Guid = guid;

            _systemDesktop = systemDesktop;
            systemDesktop.PropertyChanged += SystemDesktop_PropertyChanged;

            _background = systemDesktop.Background;
        }

        private void SystemDesktop_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_systemDesktop.Background):

                    if (!IsCurrent)
                        return;

                    UpdateBackground();
                    break;

                case nameof(_systemDesktop.Guid):
                    UpdateIsCurrent();

                    break;
                default:
                    break;
            }
        }

        private void UpdateBackground()
        {
            _background = _systemDesktop.Background;

            OnPropertyChanged(nameof(Background));
        }

        private void UpdateIsCurrent()
        {
            IsCurrent = Equals(Guid, _systemDesktop.Guid);
        }

        public override bool Equals(object obj)
        {
            var desktop = obj as IVirtualDesktop;

            if (desktop == null)
                return false;

            return Guid.Equals(desktop.Guid);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
