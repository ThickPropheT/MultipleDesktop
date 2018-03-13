using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace MultipleDesktop.Windows.PresentationFoundation.View
{
    public static class WindowExtensions
    {
        private const string User32Dll = "user32.dll";

        private const int GWL_STYLE = -16;

        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;

        [DllImport(User32Dll)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(User32Dll)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static void SetCanMinimize(this Window window, bool canMinimize)
            => SourceInitializedMediator.Mediate(
                window,
                w => SetCanMinimizeImpl(w, canMinimize));

        public static void SetCanMaximize(this Window window, bool canMaximize)
            => SourceInitializedMediator.Mediate(
                window,
                w => SetCanMaximizeImpl(w, canMaximize));

        private static IntPtr GetHandle(this Window window)
            => new WindowInteropHelper(window).Handle;

        private static void SetCanMinimizeImpl(Window window, bool canMinimize)
        {
            var hWnd = window.GetHandle();

            var currentStyle = GetWindowLong(hWnd, GWL_STYLE);

            var newStyle = canMinimize
                ? currentStyle | WS_MINIMIZEBOX
                : currentStyle & ~WS_MINIMIZEBOX;

            SetWindowLong(hWnd, GWL_STYLE, newStyle);
        }

        private static void SetCanMaximizeImpl(Window window, bool canMaximize)
        {
            var hWnd = window.GetHandle();

            var currentStyle = GetWindowLong(hWnd, GWL_STYLE);

            var newStyle = canMaximize
                ? currentStyle | WS_MAXIMIZEBOX
                : currentStyle & ~WS_MAXIMIZEBOX;

            SetWindowLong(hWnd, GWL_STYLE, newStyle);
        }
    }
}
