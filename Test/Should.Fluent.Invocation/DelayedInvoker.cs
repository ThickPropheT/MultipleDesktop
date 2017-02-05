using System;

namespace Should.Fluent.Invocation
{
    public class DelayedInvoker
    {
        private readonly bool _doesReturn;
        private readonly Delegate _code;

        public event InvocationOccurred InvocationOccurred;

        public DelayedInvoker(Action code)
        {
            _doesReturn = false;
            _code = code;
        }

        public DelayedInvoker(Func<object> code)
        {
            _doesReturn = true;
            _code = code;
        }

        public void AddListener(InvocationOccurred onInvocation)
        {
            InvocationOccurred += onInvocation;
        }

        public void Invoke()
        {
            var invocation = _doesReturn
                ? Record.Invocation((Func<object>)_code)
                : Record.Invocation((Action)_code);

            InvocationOccurred?.Invoke(invocation);
        }
    }
}
