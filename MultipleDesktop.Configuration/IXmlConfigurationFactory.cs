using MultipleDesktop.Mvc.Configuration;

namespace MultipleDesktop.Configuration
{
    public interface IXmlConfigurationFactory
    {
        VirtualDesktopConfiguration ToXmlConfiguration(IVirtualDesktopConfiguration configuration);
        AppConfiguration ToXmlConfiguration(IAppConfiguration configuration);
    }
}