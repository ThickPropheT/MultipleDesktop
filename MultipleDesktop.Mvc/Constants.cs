using MultipleDesktop.Mvc.Desktop;
using System;

namespace MultipleDesktop.Mvc
{
    public static class Constants
    {
        public static class Default
        {
            public static readonly Fit Fit = Fit.Center;

            public static class Config
            {
                public const string FileName = "MultipleDesktop.xml";
            }

            public static class Ui
            {
                public static readonly TimeSpan UpdateRate = TimeSpan.FromMilliseconds(50);
            }
        }
    }
}
