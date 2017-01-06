using MultipleDesktop.Mvc.Desktop;
using System;

namespace MultipleDesktop.Windows
{
    internal static class Registry
    {
        internal static class Desktop
        {
            /// <summary>
            /// Length of Virtual Desktop UUIDs in bytes.
            /// </summary>
            internal const int UuidLength = 16;

            internal const string SubKey = @"Control Panel\Desktop";

            internal static class VirtualDesktopIDs
            {
                internal const string SubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VirtualDesktops";

                internal const string Value = nameof(VirtualDesktopIDs);
            }

            internal static class CurrentVirtualDesktop
            {
                internal static string SubKeyFor(int sessionId)
                {
                    return $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\SessionInfo\{sessionId}\VirtualDesktops";
                }

                internal const string Value = nameof(CurrentVirtualDesktop);
            }

            internal static class TileWallpaper
            {
                internal const string Key = nameof(TileWallpaper);

                private static class Data
                {
                    internal const string NotTiled = "0";
                    internal const string Tiled = "1";
                }

                internal static string DataFromFit(Fit fit)
                {
                    return fit == Fit.Tile
                        ? Data.Tiled
                        : Data.NotTiled;
                }
            }

            internal static class WallpaperStyle
            {
                internal const string Key = nameof(WallpaperStyle);

                private static class Data
                {
                    internal static readonly string TileOrCenter = "0";
                    internal static readonly string Stretch = "2";
                    internal static readonly string Fit = "6";
                    internal static readonly string Fill = "7";
                }

                internal static string DataFromFit(Fit fit)
                {
                    switch (fit)
                    {
                        case Fit.Tile:
                        case Fit.Center:
                            return Data.TileOrCenter;
                        case Fit.Stretch:
                            return Data.Stretch;
                        case Fit.Fit:
                            return Data.Fit;
                        case Fit.Fill:
                            return Data.Fill;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }
    }
}
