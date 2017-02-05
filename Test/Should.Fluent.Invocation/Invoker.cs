using System;

namespace Should.Fluent.Invocation
{
    public class Invoker
    {
        private readonly bool _doesReturn;
        private readonly Delegate _code;

        public Invoker(Action code)
        {
            _doesReturn = false;
            _code = code;
        }

        public Invoker(Func<object> code)
        {
            _doesReturn = true;
            _code = code;
        }

        public Invocation.Result Invoke()
        {
            return _doesReturn
                ? Record.Invocation((Func<object>)_code)
                : Record.Invocation((Action)_code);
        }
    }
}