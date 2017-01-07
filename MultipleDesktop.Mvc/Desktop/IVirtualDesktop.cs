using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Desktop
{
    public interface IVirtualDesktop : INotifyPropertyChanged
    {
        Guid Guid { get; }
        uint Index { get; }

        bool IsCurrent { get; }

        IBackground Background { get; set; }
    }
}
