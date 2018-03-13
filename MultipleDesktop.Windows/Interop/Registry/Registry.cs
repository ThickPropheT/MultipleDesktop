using System;
using uFit = MultipleDesktop.Mvc.Desktop.Fit;

namespace MultipleDesktop.Windows.Interop.Registry
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
                    => $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\SessionInfo\{sessionId}\VirtualDesktops";

                internal const string Value = nameof(CurrentVirtualDesktop);
            }

            internal static class TileWallpaper
            {
                internal const string Value = nameof(TileWallpaper);

                internal static class Data
                {
                    internal const byte NotTiled = 0;
                    internal const byte Tiled = 1;
                }

                internal static byte DataFromFit(uFit fit)
                    => fit == uFit.Tile
                        ? Data.Tiled
                        : Data.NotTiled;
            }

            internal static class WallpaperStyle
            {
                internal const string Value = nameof(WallpaperStyle);

                internal static class Data
                {
                    internal static readonly byte TileOrCenter = 0;
                    internal static readonly byte Stretch = 2;
                    internal static readonly byte Fit = 6;
                    internal static readonly byte Fill = 10;
                    internal static readonly byte Span = 22;
                }

                internal static byte DataFromFit(uFit fit)
                {
                    switch (fit)
                    {
                        case uFit.Tile:
                        case uFit.Center:
                            return Data.TileOrCenter;
                        case uFit.Stretch:
                            return Data.Stretch;
                        case uFit.Fit:
                            return Data.Fit;
                        case uFit.Fill:
                            return Data.Fill;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }

            internal static class Fit
            {
                internal static uFit FromData(byte tile, byte style)
                {
                    switch (style)
                    {
                        case 0:
                            return tile == 1
                                ? uFit.Tile
                                : uFit.Center;
                        case 2:
                            return uFit.Stretch;
                        case 6:
                            return uFit.Fit;
                        case 10:
                            return uFit.Fill;
                        case 22:
                            return uFit.Span;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }
    }
}
