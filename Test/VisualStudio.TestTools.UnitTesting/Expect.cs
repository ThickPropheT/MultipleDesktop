using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace VisualStudio.TestTools.UnitTesting
{
    public static class Expect
    {
        public static void Exception(Exception e, Action fromScope)
        {
            try
            {
                fromScope();

                Assert.Fail($"Expected exception '{e}'");
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (!ReferenceEquals(ex, e))
                    Assert.Fail($"Expected exception '{e}', but encountered '{ex}'");
            }
        }

        public static void Exception<T>(Action fromScope) where T : Exception
        {
            try
            {
                fromScope();

                Assert.Fail($"Expected exception of type '{typeof(T).FullName}'");
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (!(ex is T))
                    Assert.Fail($"Expected exception of type '{typeof(T).FullName}', but encountered '{ex}'");
            }
        }
    }
}
