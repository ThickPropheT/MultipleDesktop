using System;

namespace Should.Fluent.Invocation
{
    [Serializable]
    public class UnexpectedExceptionException : Exception
    {
        public UnexpectedExceptionException() { }
        public UnexpectedExceptionException(string message) : base(message) { }
        public UnexpectedExceptionException(string message, Exception inner) : base(message, inner) { }
        protected UnexpectedExceptionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
