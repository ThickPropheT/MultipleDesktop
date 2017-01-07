using System.Runtime.InteropServices;

namespace MultipleDesktop.Windows.Interop.Registry
{
    // named as per SystemParametersInfo function documentation https://msdn.microsoft.com/en-us/library/windows/desktop/ms724947(v=vs.85).aspx
    // implemented as per Design Warning CA1060 https://msdn.microsoft.com/en-us/library/ms182161.aspx
    internal sealed class NativeMethods
    {
        private NativeMethods() { }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool SystemParametersInfo(
            int uiAction,
            int uiParam,
            string pvParam,
            int fWinIni);

        internal const int SPI_GETDESKWALLPAPER = 0x0073;
        internal const int SPI_SETDESKWALLPAPER = 0x0014;

        internal const int SPIF_UPDATEINIFILE = 0x01;
        internal const int SPIF_SENDCHANGE = 0x02;
    }
}
