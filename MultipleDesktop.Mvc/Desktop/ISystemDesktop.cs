using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Desktop
{
    public interface ISystemDesktop : INotifyPropertyChanged
    {
        Guid Guid { get; }

        IBackground Background { get; set; }
    }
}
