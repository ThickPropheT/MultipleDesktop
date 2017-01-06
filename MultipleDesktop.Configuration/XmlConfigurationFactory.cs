using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using System.Collections.Generic;

namespace MultipleDesktop.Configuration
{
    public class XmlConfigurationFactory : IConfigurationFactory, IXmlConfigurationFactory
    {
        public IVirtualDesktopConfiguration ConfigurationFor(IVirtualDesktop desktop)
        {
            return new VirtualDesktopConfiguration(desktop, this);
        }

        public IBackground BackgroundFrom(IVirtualDesktopConfiguration configuration)
        {
            return new Background(configuration.BackgroundPath, configuration.Fit);
        }

        public IAppConfiguration AppConfigurationFor(IEnumerable<IVirtualDesktopConfiguration> configurations)
        {
            return new AppConfiguration(configurations, this);
        }

        public IBackground BackgroundFrom(string backgroundPath, Fit fit)
        {
            return new Background(backgroundPath, fit);
        }

        public VirtualDesktopConfiguration ToXmlConfiguration(IVirtualDesktopConfiguration configuration)
        {
            return configuration as VirtualDesktopConfiguration
                ?? new VirtualDesktopConfiguration(configuration.TargetDesktop, this);
        }

        public AppConfiguration ToXmlConfiguration(IAppConfiguration configuration)
        {
            return configuration as AppConfiguration
                ?? new AppConfiguration(configuration.GetAll(), this);
        }
    }
}
