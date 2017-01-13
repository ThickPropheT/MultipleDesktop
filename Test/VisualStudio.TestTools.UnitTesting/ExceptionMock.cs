using System;

namespace VisualStudio.TestTools.UnitTesting
{
    [Serializable]
    public class ExceptionMock : Exception
    {
        public ExceptionMock() { }
        public ExceptionMock(string message) : base(message) { }
        public ExceptionMock(string message, Exception inner) : base(message, inner) { }
        protected ExceptionMock(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
