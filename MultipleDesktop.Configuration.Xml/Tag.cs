namespace MultipleDesktop.Configuration.Xml
{
    public static class Tag
    {
        public static class AppConfiguration
        {
            public const string Name = "app";

            public static class DesktopConfigurations
            {
                public const string ArrayItemName = "desktop";
                public const string ArrayName = "desktops";
            }
        }

        public static class VirtualDesktopConfiguration
        {
            public const string Name = "desktop";
            public const string Guid = "guid";
            public const string BackgroundPath = "background-path";
            public const string Fit = "fit";
        }
    }
}
