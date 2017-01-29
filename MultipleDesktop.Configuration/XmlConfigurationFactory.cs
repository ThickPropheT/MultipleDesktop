using MultipleDesktop.Configuration.Xml;
using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization.Extended;
using System.ComponentModel;
using MultipleDesktop.Mvc;

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

        public IAppConfiguration AppConfigurationFrom(IEnumerable<IVirtualDesktopConfiguration> configurations)
        {
            return new AppConfiguration(configurations.Select(ToXmlConfiguration));
        }

        public IBackground BackgroundFrom(string backgroundPath, Fit fit)
        {
            return new Background(backgroundPath, fit);
        }

        public IVirtualDesktop DesktopFrom(Guid guid, uint index, ISystemDesktop systemDesktop)
        {
            return new VirtualDesktop(guid, index, systemDesktop);
        }

        public IXmlSerializer CreateSerializerFor<T>()
        {
            return new XmlSerializer(typeof(T));
        }

        public TextWriter CreateSerializationWriterFor(string path)
        {
            return new StreamWriter(path);
        }

        public TextReader CreateSerializationReaderFor(string path)
        {
            return new StreamReader(path);
        }

        public AppConfiguration CreateXmlConfiguration()
        {
            return new AppConfiguration();
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
                ?? new AppConfiguration(configuration.GetAll().Select(ToXmlConfiguration));

        }

        public IPropertyChangedBinding Bind(Action target, INotifyPropertyChanged toSource)
        {
            return new PropertyChangedBinding(toSource, target);
        }
    }
}
