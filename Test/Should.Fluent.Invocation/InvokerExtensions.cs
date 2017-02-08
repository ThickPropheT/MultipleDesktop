using Should.Fluent.Model;
using System;

namespace Should.Fluent.Invocation
{
    public static class InvokerExtensions
    {
        public static Invoker Throw<TException>(this IShould<Invoker> should)
        {
            var exceptionType = typeof(TException);

            return should.Apply(
                (t, a) =>
                {
                    var result = t.Invoke();

                    if (result.Error == null)
                        a.Fail(AssertionMessages.NoDerivedExceptionMessage(exceptionType));

                    if (!(result.Error is TException))
                        a.Fail(AssertionMessages.NoDerivedExceptionMessage(exceptionType, result.Error));
                },
                (t, a) =>
                {
                    var result = t.Invoke();

                    if (result.Error is TException)
                        a.Fail(AssertionMessages.DerivedExceptionMessage(exceptionType));

                    if (result.Error != null)
                        throw new UnexpectedExceptionException(AssertionMessages.UnexpectedExceptionMessage, result.Error);
                });
        }

        public static Invoker Throw(this IShould<Invoker> should, Type exceptionType)
        {
            return should.Apply(
                (t, a) =>
                {
                    var result = t.Invoke();

                    if (result.Error == null)
                        a.Fail(AssertionMessages.NoExactExceptionMessage(exceptionType));

                    if (result.Error.GetType() != exceptionType)
                        a.Fail(AssertionMessages.NoExactExceptionMessage(exceptionType, result.Error));
                },
                (t, a) =>
                {
                    var result = t.Invoke();

                    if (exceptionType == result.Error.GetType())
                        a.Fail(AssertionMessages.ExactExceptionMessage(exceptionType));

                    if (result.Error != null)
                        throw new UnexpectedExceptionException(AssertionMessages.UnexpectedExceptionMessage, result.Error);
                });
        }

        public static Invoker Throw(this IShould<Invoker> should, Exception error)
        {
            return should.Apply(
                (t, a) =>
                {
                    var result = t.Invoke();

                    if (result.Error == null)
                        a.Fail(AssertionMessages.NoExceptionInstanceMessage(error));

                    if (!result.Error.Equals(error))
                        a.Fail(AssertionMessages.NoExceptionInstanceMessage(error, result.Error));
                },
                (t, a) =>
                {
                    var result = t.Invoke();

                    if (error.Equals(result.Error))
                        a.Fail(AssertionMessages.ExceptionInstanceMessage(error));

                    if (result.Error != null)
                        throw new UnexpectedExceptionException(AssertionMessages.UnexpectedExceptionMessage, result.Error);
                });
        }
    }
}
