using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Desktop
{
    public interface IVirtualDesktopState : INotifyPropertyChanging, INotifyPropertyChanged
    {
        IVirtualDesktop Current { get; }

        IEnumerable<IVirtualDesktop> AllDesktops { get; }

        void Load();
    }
}
