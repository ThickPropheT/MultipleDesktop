using MultipleDesktop.Mvc.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MultipleDesktop.Configuration.Xml
{
    [XmlType("app")]
    public class AppConfiguration : IAppConfiguration
    {
        private List<VirtualDesktopConfiguration> _desktopConfigurations;

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

        /// <summary>
        /// Xml serialization constructor.
        /// </summary>
        public AppConfiguration()
        {
            _desktopConfigurations = new List<VirtualDesktopConfiguration>();
        }

        public AppConfiguration(IEnumerable<VirtualDesktopConfiguration> configurations)
        {
            _desktopConfigurations = new List<VirtualDesktopConfiguration>(configurations);
        }

        public IEnumerable<IVirtualDesktopConfiguration> GetAll()
        {
            return _desktopConfigurations;
        }
    }
}
