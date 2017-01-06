using MultipleDesktop.Mvc.Desktop;
using System.Collections.Generic;

namespace MultipleDesktop.Mvc.Configuration
{
    public interface IConfigurationFactory
    {
        IVirtualDesktopConfiguration ConfigurationFor(IVirtualDesktop desktop);
        IAppConfiguration AppConfigurationFor(IEnumerable<IVirtualDesktopConfiguration> configurations);
        IBackground BackgroundFrom(IVirtualDesktopConfiguration configuration);
        IBackground BackgroundFrom(string backgroundPath, Fit fit);
    }
}
