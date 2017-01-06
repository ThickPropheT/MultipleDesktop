using MultipleDesktop.Mvc.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MultipleDesktop.Configuration
{
    [XmlType("app")]
    public class AppConfiguration : IAppConfiguration
    {
        private List<VirtualDesktopConfiguration> _desktopConfigurations = new List<VirtualDesktopConfiguration>();

        [XmlArrayItem("desktop")]
        [XmlArray("desktops")]
        public VirtualDesktopConfiguration[] DesktopConfigurations
        {
            get
            {
                return _desktopConfigurations.Any()
                    ? _desktopConfigurations.ToArray()
                    : null;
            }
            set
            {
                if (value == null)
                    return;

                _desktopConfigurations = new List<VirtualDesktopConfiguration>(value);
            }
        }

        public AppConfiguration()
        {

        }

        public AppConfiguration(IEnumerable<IVirtualDesktopConfiguration> configurations, IXmlConfigurationFactory factory)
        {
            _desktopConfigurations.AddRange(
                configurations
                    .Select(configuration =>
                        factory.ToXmlConfiguration(configuration)));
        }

        public IEnumerable<IVirtualDesktopConfiguration> GetAll()
        {
            return _desktopConfigurations;
        }
    }
}
