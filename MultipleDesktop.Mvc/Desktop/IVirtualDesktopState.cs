using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Desktop
{
    public static class PropertyChangingEventArgsExtensions
    {
        public static bool ShouldRollback(this System.ComponentModel.PropertyChangingEventArgs args)
        {
            return (args as PropertyChangingEventArgs)?.Exception != null;
        }

        public static bool ShouldRollback(this System.ComponentModel.PropertyChangingEventArgs args, out Exception exception)
        {
            var rollbackArgs = args as PropertyChangingEventArgs;

            exception = rollbackArgs?.Exception;

            return exception != null;
        }
    }

    public class PropertyChangingEventArgs : System.ComponentModel.PropertyChangingEventArgs
    {
        public Exception Exception { get; }

        public PropertyChangingEventArgs(string propertyName, Exception exception)
            : base(propertyName)
        {
            Exception = exception;
        }
    }

    public interface IVirtualDesktopState : INotifyPropertyChanging, INotifyPropertyChanged
    {
        IVirtualDesktop Current { get; }

        IEnumerable<IVirtualDesktop> AllDesktops { get; }

        void Load();
    }
}
