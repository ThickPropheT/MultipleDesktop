using System;

namespace Should.Fluent.Invocation
{
    public static class Invoke
    {
        public static Invoker Delegate(Action code)
        {
            return new Invoker(code);
        }

        public static Invoker Delegate(Func<object> code)
        {
            return new Invoker(code);
        }
    }
}
