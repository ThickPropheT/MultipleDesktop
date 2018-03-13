using System;
using System.Runtime.InteropServices;

namespace MultipleDesktop.Windows.Interop.Shell
{
    // named as per SystemParametersInfo function documentation https://msdn.microsoft.com/en-us/library/windows/desktop/ms724947(v=vs.85).aspx
    // implemented as per Design Warning CA1060 https://msdn.microsoft.com/en-us/library/ms182161.aspx
    internal sealed class NativeMethods
    {
        private NativeMethods() { }

        internal static TResult CreateInstance<TCoClass, TResult>()
            => (TResult)Activator.CreateInstance(Type.GetTypeFromCLSID(typeof(TCoClass).GUID));

        internal static TResult CreateInstance<TResult>(Guid clsid)
            => (TResult)Activator.CreateInstance(Type.GetTypeFromCLSID(clsid));

        internal static class CLSID
        {
            internal static readonly Guid DesktopWallpaper = new Guid("C2CF3110-460E-4fc1-B9D0-8A1C0C9CC4BD");

            internal static readonly Guid VirtualDesktopManager = new Guid("AA509086-5CA9-4C25-8F95-589D3C07B48A");
            internal static readonly Guid ImmersiveShell = new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239");

            internal static readonly Guid VirtualNotificationService = new Guid("A501FDEC-4A09-464C-AE4E-1B9C21B84918");
        }

        internal static class IID
        {
            /// <summary>
            /// Not sure what service this IID represents.
            /// Source : https://github.com/Grabacr07/VirtualDesktop
            /// </summary>
            internal static Guid VirtualDesktopAPIUnknown = new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B");
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
        internal interface IVirtualDesktopManager
        {
            int IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow);
            Guid GetWindowDesktopId(IntPtr topLevelWindow);
            void MoveWindowToDesktop(IntPtr topLevelWindow, ref Guid desktopId);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("C179334C-4295-40D3-BEA1-C654D965605A")]
        internal interface IVirtualDesktopNotification
        {
            int IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow);
            Guid GetWindowDesktopId(IntPtr topLevelWindow);
            void MoveWindowToDesktop(IntPtr topLevelWindow, ref Guid desktopId);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
        internal interface IServiceProvider
        {
            [PreserveSig]
            [return: MarshalAs(UnmanagedType.I4)]
            int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
        internal interface IObjectArray
        {
            void GetCount(out int count);
            void GetAt(int index, ref Guid riid, [MarshalAs(UnmanagedType.Interface)]out object obj);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        /*
        [Guid("AF8DA486-95BB-4460-B3B7-6E7A6B2962B5")]
        */
        [Guid("F31574D6-B682-4CDC-BD56-1827860ABEC6")]
        internal interface IVirtualDesktopManagerInternal
        {
            int GetCount();
            void notimpl1();  // void MoveViewToDesktop(IApplicationView view, IVirtualDesktop desktop);
            void notimpl2();  // void CanViewMoveDesktops(IApplicationView view, out int itcan);
            IVirtualDesktop GetCurrentDesktop();
            void GetDesktops(out IObjectArray desktops);
            [PreserveSig]
            int GetAdjacentDesktop(IVirtualDesktop from, int direction, out IVirtualDesktop desktop);
            void SwitchDesktop(IVirtualDesktop desktop);
            IVirtualDesktop CreateDesktop();
            void RemoveDesktop(IVirtualDesktop desktop, IVirtualDesktop fallback);
            IVirtualDesktop FindDesktop(ref Guid desktopid);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4")]
        internal interface IVirtualDesktop
        {
            void notimpl1(); // void IsViewVisible(IApplicationView view, out int visible);
            Guid GetId();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal int left, top, right, bottom;
        }

        internal enum SIGDN : uint
        {
            NORMALDISPLAY = 0,
            PARENTRELATIVEPARSING = 0x80018001,
            PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
            DESKTOPABSOLUTEPARSING = 0x80028000,
            PARENTRELATIVEEDITING = 0x80031001,
            DESKTOPABSOLUTEEDITING = 0x8004c000,
            FILESYSPATH = 0x80058000,
            URL = 0x80068000
        }

        [Flags]
        internal enum DESKTOP_SLIDESHOW_DIRECTION : int
        {
            DSD_FORWARD = 0,
            DSD_BACKWARD = 1
        }

        [Flags]
        internal enum DESKTOP_WALLPAPER_POSITION : int
        {
            DWPOS_CENTER = 0,
            DWPOS_TILE = 1,
            DWPOS_STRETCH = 2,
            DWPOS_FIT = 3,
            DWPOS_FILL = 4,
            DWPOS_SPAN = 5
        }

        [Flags]
        internal enum DESKTOP_SLIDESHOW_OPTIONS : int
        {
            DSO_SHUFFLEIMAGES = 0x1
        }

        [Flags]
        internal enum DESKTOP_SLIDESHOW_STATE : int
        {
            DSS_ENABLED = 0x1,
            DSS_SLIDESHOW = 0x2,
            DSS_DISABLED_BY_REMOTE_SESSION = 0x4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COLORREF
        {
            internal byte R;
            internal byte G;
            internal byte B;
        }

        //[ComImport]
        //[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        //[Guid("B92B56A9-8B55-4E14-9A89-0199BBB6F93B")]
        //internal interface IDesktopWallpaper
        //{
        //    void AdvanceSlideshow(
        //        [In]  string monitorID,
        //        [In]  DESKTOP_SLIDESHOW_DIRECTION direction);

        //    void Enable(out bool enable); /*use "out" or get AccessViolationException */

        //    void GetBackgroundColor([Out]  COLORREF color);

        //    int GetMonitorDevicePathAt(
        //        [In] uint monitorIndex,
        //        [In, Out] ref string monitorID);

        //    void GetMonitorDevicePathCount(ref uint count);

        //    void GetMonitorRECT([In] string monitorID, [Out]  RECT displayRect);

        //    void GetPosition(
        //        [Out] out DESKTOP_WALLPAPER_POSITION position);

        //    void GetSlideshow([Out] object items);
        //    /*void GetSlideshow([Out]  IShellItemArray items); */

        //    void GetSlideshowOptions([Out]  DESKTOP_SLIDESHOW_OPTIONS options, [Out]  uint slideshowTick);

        //    void GetStatus([Out]  DESKTOP_SLIDESHOW_STATE state);

        //    int GetWallpaper(
        //        [In] string monitorID,
        //        [In, Out] ref string wallpaper);

        //    void SetBackgroundColor([In] COLORREF color);

        //    void SetPosition([In]  DESKTOP_WALLPAPER_POSITION position);

        //    void SetSlideshow([In]  object items);
        //    /* void  SetSlideshow([In]  IShellItemArray items); */

        //    void SetSlideshowOptions([In]  DESKTOP_SLIDESHOW_OPTIONS options, [In]  uint slideshowTick);

        //    void SetWallpaper(
        //        [In] string monitorID,
        //        [In] string wallpaper);
        //}

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("B92B56A9-8B55-4E14-9A89-0199BBB6F93B")]
        internal interface IDesktopWallpaper
        {
            int SetWallpaper(
                [In, MarshalAs(UnmanagedType.LPWStr)] string monitorID,
                [In, MarshalAs(UnmanagedType.LPWStr)] string wallpaper);

            int GetWallpaper(
                [In] string monitorID,
                [In, Out, MarshalAs(UnmanagedType.LPWStr)] ref string wallpaper);

            int GetMonitorDevicePathAt(
                [In] uint monitorIndex,
                [In, Out, MarshalAs(UnmanagedType.LPWStr)] ref string monitorID);

            int GetMonitorDevicePathCount(
                [In, Out] ref uint count);

            void GetMonitorRECT([In] string monitorID, [Out]  RECT displayRect);

            void SetBackgroundColor([In] COLORREF color);

            void GetBackgroundColor([Out]  COLORREF color);

            int SetPosition(
                [In]  DESKTOP_WALLPAPER_POSITION position);

            void GetPosition(
                [Out] out DESKTOP_WALLPAPER_POSITION position);

            void SetSlideshow([In]  object items);
            /* void  SetSlideshow([In]  IShellItemArray items); */

            void GetSlideshow([Out] object items);
            /*void GetSlideshow([Out]  IShellItemArray items); */

            void SetSlideshowOptions([In]  DESKTOP_SLIDESHOW_OPTIONS options, [In]  uint slideshowTick);

            void GetSlideshowOptions([Out]  DESKTOP_SLIDESHOW_OPTIONS options, [Out]  uint slideshowTick);

            void AdvanceSlideshow(
                [In]  string monitorID,
                [In]  DESKTOP_SLIDESHOW_DIRECTION direction);

            void GetStatus([Out]  DESKTOP_SLIDESHOW_STATE state);

            void Enable(out bool enable); /*use "out" or get AccessViolationException */
        }
    }
}
