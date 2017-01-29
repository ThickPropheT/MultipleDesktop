using MultipleDesktop.Mvc.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Configuration
{
    public interface IConfigurationFactory
    {
        IVirtualDesktopConfiguration ConfigurationFor(IVirtualDesktop desktop);
        IAppConfiguration AppConfigurationFrom(IEnumerable<IVirtualDesktopConfiguration> configurations);
        IBackground BackgroundFrom(string backgroundPath, Fit fit);
        IVirtualDesktop DesktopFrom(Guid guid, uint index, ISystemDesktop systemDesktop);
        IPropertyChangedBinding Bind(Action target, INotifyPropertyChanged toSource);
    }
}
