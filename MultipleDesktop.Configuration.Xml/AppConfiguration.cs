using MultipleDesktop.Mvc.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using XmlTag = MultipleDesktop.Configuration.Xml.Tag.AppConfiguration;

namespace MultipleDesktop.Configuration.Xml
{
    [XmlType(XmlTag.Name)]
    public class AppConfiguration : IAppConfiguration
    {
        private IEnumerable<VirtualDesktopConfiguration> _desktopConfigurations;

        [XmlArrayItem(XmlTag.DesktopConfigurations.ArrayItemName)]
        [XmlArray(XmlTag.DesktopConfigurations.ArrayName)]
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

                _desktopConfigurations = value;
            }
        }

        /// <summary>
        /// Xml serialization constructor.
        /// </summary>
        public AppConfiguration()
        {
            _desktopConfigurations = Enumerable.Empty<VirtualDesktopConfiguration>();
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
