using MultipleDesktop.Mvc.Configuration;
using static MultipleDesktop.Windows.Interop.Shell.NativeMethods;

namespace MultipleDesktop.Windows.Interop.Shell
{
    public class WindowsShellFactory
    {
        public static IWindowsDesktopAdapter CreateAdapter(IConfigurationFactory configurationFactory)
        {
            var desktop = CreateInstance<IDesktopWallpaper>(CLSID.DesktopWallpaper);

            var shell = CreateInstance<IServiceProvider>(CLSID.ImmersiveShell);

            object managerResult;
            shell.QueryService(
                GUID.VirtualDesktopAPIUnknown,
                typeof(IVirtualDesktopManagerInternal).GUID,
                out managerResult);

            return new WindowsShellAdapter(
                configurationFactory,
                desktop,
                (IVirtualDesktopManagerInternal)managerResult);
        }
    }
}
