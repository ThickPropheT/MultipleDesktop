using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using System.Collections.Generic;

namespace MultipleDesktop.Configuration
{
    public class XmlConfigurationFactory : IConfigurationFactory, IXmlConfigurationFactory
    {
        /// <summary>
        /// Creates a new <see cref="IVirtualDesktopConfiguration"/> using
        /// the data values of <paramref name="desktop"/>.
        /// </summary>
        /// <param name="desktop">The <see cref="IVirtualDesktop"/> for which to create configuration.</param>
        /// <returns>The <see cref="IVirtualDesktopConfiguration"/> for <paramref name="desktop"/>.</returns>
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

        /// <summary>
        /// If <paramref name="configuration"/> is in Xml format, casts it to the
        /// Xml serializable type, <see cref="VirtualDesktopConfiguration"/>, otherwise
        /// creates a new Xml serializable <see cref="VirtualDesktopConfiguration"/>
        /// from <paramref name="configuration"/>'s data values.
        /// </summary>
        /// <param name="configuration">The <see cref="IVirtualDesktopConfiguration"/> to convert to Xml format.</param>
        /// <returns>An Xml serializable representation of <paramref name="configuration"/>.</returns>
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
