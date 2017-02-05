using Should.Fluent.Model;
using System;

namespace Should.Fluent.Invocation
{
    public static class DelayedInvokerExtensions
    {
        public static DelayedInvoker Throw<TException>(this IShould<DelayedInvoker> should)
        {
            var exceptionType = typeof(TException);

            return should.Apply(
                (t, a) => t.AddListener(i =>
                {
                    if (i.Error == null)
                        a.Fail(AssertionMessages.NoDerivedExceptionMessage(exceptionType));

                    if (!(i.Error is TException))
                        a.Fail(AssertionMessages.NoDerivedExceptionMessage(exceptionType, i.Error));
                }),
                (t, a) => t.AddListener(i =>
                {
                    if (i.Error is TException)
                        a.Fail(AssertionMessages.DerivedExceptionMessage(exceptionType));

                    throw new UnexpectedExceptionException(AssertionMessages.UnexpectedExceptionMessage, i.Error);
                }));
        }

        public static DelayedInvoker Throw(this IShould<DelayedInvoker> should, Type exceptionType)
        {
            return should.Apply(
                (t, a) => t.AddListener(i =>
                {
                    if (i.Error == null)
                        a.Fail(AssertionMessages.NoExactExceptionMessage(exceptionType));

                    if (i.Error.GetType() != exceptionType)
                        a.Fail(AssertionMessages.NoExactExceptionMessage(exceptionType, i.Error));
                }),
                (t, a) => t.AddListener(i =>
                {
                    if (exceptionType == i.Error.GetType())
                        a.Fail(AssertionMessages.ExactExceptionMessage(exceptionType));

                    throw new UnexpectedExceptionException(AssertionMessages.UnexpectedExceptionMessage, i.Error);
                }));
        }

        public static DelayedInvoker Throw(this IShould<DelayedInvoker> should, Exception error)
        {
            return should.Apply(
                (t, a) => t.AddListener(i =>
                {
                    if (i.Error == null)
                        a.Fail(AssertionMessages.NoExceptionInstanceMessage(error));

                    if (!i.Error.Equals(error))
                        a.Fail(AssertionMessages.NoExceptionInstanceMessage(error, i.Error));
                }),
                (t, a) => t.AddListener(i =>
                {
                    if (error.Equals(i.Error))
                        a.Fail(AssertionMessages.ExceptionInstanceMessage(error));

                    throw new UnexpectedExceptionException(AssertionMessages.UnexpectedExceptionMessage, i.Error);
                }));
        }
    }
}
