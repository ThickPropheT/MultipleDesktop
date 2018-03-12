using MultipleDesktop.Mvc.Configuration;
using System.Collections.Generic;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Controller
{
    public interface IAppController : INotifyPropertyChanged
    {
        IEnumerable<IVirtualDesktopConfiguration> DesktopConfigurations { get; }

        void Load();
        void Save();
    }
}
