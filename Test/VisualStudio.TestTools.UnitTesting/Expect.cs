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
            catch (AssertFailedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                if (!ReferenceEquals(ex, e))
                    Assert.Fail($"Expected exception '{e}', but encountered '{ex}'");
            }
        }
    }
}
