using System;

namespace MultipleDesktop.Mvc.Desktop
{
    public class PropertyRollBackChangesEventArgs : System.ComponentModel.PropertyChangingEventArgs
    {
        public Exception Exception { get; }

        public PropertyRollBackChangesEventArgs(string propertyName, Exception exception)
            : base(propertyName)
        {
            Exception = exception;
        }
    }
}
