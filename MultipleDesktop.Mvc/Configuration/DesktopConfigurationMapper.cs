using MultipleDesktop.Mvc.Desktop;

namespace MultipleDesktop.Mvc.Configuration
{
    public static class DesktopConfigurationMapper
    {
        public static bool IsConfigurationFor(this IVirtualDesktopConfiguration configuration, IVirtualDesktop desktop)
        {
            return configuration.Guid.Equals(desktop.Guid);
        }
    }
}
