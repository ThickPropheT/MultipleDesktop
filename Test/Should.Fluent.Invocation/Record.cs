using System;

namespace Should.Fluent.Invocation
{
    using oInvocation = Invocation;

    public static class Record
    {
        public static oInvocation.Result Invocation(Action code)
        {
            try
            {
                code();
                return oInvocation.Result.Succeeded();
            }
            catch (Exception ex)
            {
                return oInvocation.Result.FailedWith(ex);
            }
        }

        public static oInvocation.Result Invocation(Func<object> code)
        {
            try
            {
                var result = code();
                return oInvocation.Result.Succeeded(result);
            }
            catch (Exception ex)
            {
                return oInvocation.Result.FailedWith(ex);
            }
        }
    }
}
