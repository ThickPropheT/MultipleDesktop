using MultipleDesktop.Mvc.Desktop;
using static MultipleDesktop.Windows.Interop.Shell.NativeMethods;

namespace MultipleDesktop.Windows.Interop.Shell
{
    internal static class FitConverter
    {
        internal static DESKTOP_WALLPAPER_POSITION ToPosition(this Fit fit)
            => (DESKTOP_WALLPAPER_POSITION)fit;

        internal static Fit ToFit(this DESKTOP_WALLPAPER_POSITION position)
            => (Fit)position;
    }
}
