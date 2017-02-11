using System;

namespace MultipleDesktop.Configuration.Xml
{
    [Serializable]
    public class NoTargetBoundException : Exception
    {
        public string MemberName { get; }

        public NoTargetBoundException(string memberName, string message)
            : base(GetMessageFrom(memberName, message))
        {
            MemberName = memberName;
        }

        public NoTargetBoundException(Exception inner, string memberName, string message)
            : base(GetMessageFrom(memberName, message), inner)
        {
            MemberName = memberName;
        }

        protected NoTargetBoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public static NoTargetBoundException InvocationRequiresTarget(string memberName, string message)
        {
            return new NoTargetBoundException(memberName, message);
        }

        private static string GetMessageFrom(string memberName, string message = null)
        {
            if (message == null)
                message = string.Empty;

            else
                message = $" {message}";

            return $"The member '{memberName}' cannot be invoked until its owner has been bound to a target. {message}";
        }
    }
}
