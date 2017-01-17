using MultipleDesktop.Mvc.Configuration;
using System.IO;
using System.Xml.Serialization.Extended;

namespace MultipleDesktop.Configuration
{
    public interface IXmlConfigurationFactory
    {
        IXmlSerializer CreateSerializerFor<T>();
        TextWriter CreateSerializationWriterFor(string path);
        TextReader CreateSerializationReaderFor(string path);

        AppConfiguration CreateXmlConfiguration();
        VirtualDesktopConfiguration ToXmlConfiguration(IVirtualDesktopConfiguration configuration);
        AppConfiguration ToXmlConfiguration(IAppConfiguration configuration);
    }
}