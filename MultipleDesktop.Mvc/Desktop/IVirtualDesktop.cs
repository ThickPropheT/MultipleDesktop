using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Desktop
{
    public interface IVirtualDesktop : INotifyPropertyChanged
    {
        Guid Guid { get; }

        bool IsCurrent { get; }

        IBackground Background { get; set; }
    }
}
