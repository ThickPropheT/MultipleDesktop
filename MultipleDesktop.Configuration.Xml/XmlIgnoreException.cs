using System;
using System.Runtime.CompilerServices;

namespace MultipleDesktop.Configuration.Xml
{
    [Serializable]
    public class XmlIgnoreException : Exception
    {
        public string PropertyName { get; }

        public XmlIgnoreException([CallerMemberName] string propertyName = null)
            : base(GetMessageFrom(propertyName))
        {
            PropertyName = propertyName;
        }

        public XmlIgnoreException(string propertyName, string message)
            : base(GetMessageFrom(propertyName, message))
        {
            PropertyName = propertyName;
        }

        public XmlIgnoreException(Exception inner, [CallerMemberName] string propertyName = null)
            : base(GetMessageFrom(propertyName), inner)
        {
            PropertyName = propertyName;
        }

        public XmlIgnoreException(Exception inner, string propertyName, string message)
            : base(GetMessageFrom(propertyName, message), inner)
        {
            PropertyName = propertyName;
        }

        protected XmlIgnoreException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private static string GetMessageFrom(string propertyName, string message = null)
        {
            if (message == null)
                message = string.Empty;

            else
                message = $" {message}";

            return $"The property '{propertyName}' cannot be set during Xml deserailization.{message}";
        }
    }
}
