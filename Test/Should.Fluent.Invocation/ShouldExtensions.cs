using Should.Fluent.Model;

namespace Should.Fluent.Invocation
{
    public static class ShouldExtensions
    {
        public static IAssertProvider AssertProvider => Fluent.ShouldExtensions.AssertProvider;

        public static Should<Invoker, BeBase<Invoker>> Should(this Invoker i)
        {
            return new Should<Invoker, BeBase<Invoker>>(i, AssertProvider);
        }

        public static Should<DelayedInvoker, BeBase<DelayedInvoker>> Should(this DelayedInvoker i)
        {
            return new Should<DelayedInvoker, BeBase<DelayedInvoker>>(i, AssertProvider);
        }
    }
}
