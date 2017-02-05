using System;

namespace Should.Fluent.Invocation
{
    internal static class AssertionMessages
    {
        internal static string NoDerivedExceptionMessage(Type baseType)
        {
            return $"Expected exception with type derived from '{baseType}', but no exception was encountered.";
        }

        internal static string NoDerivedExceptionMessage(Type expectedBaseType, Exception encountered)
        {
            return $"Expected exception with type derived from '{expectedBaseType}', but encountered '{encountered}'.";
        }

        internal static string DerivedExceptionMessage(Type baseType)
        {
            return $"Expected not to encounter exception with type derived from '{baseType}', but did.";
        }

        internal static string NoExactExceptionMessage(Type exactType)
        {
            return $"Expected exception with type matching '{exactType}', but no exception was encountered.";
        }

        internal static string NoExactExceptionMessage(Type expectedExactType, Exception encountered)
        {
            return $"Expected exception with type matching '{expectedExactType}', but encountered '{encountered}'.";
        }

        internal static string ExactExceptionMessage(Type exactType)
        {
            return $"Expected not to encounter exception with type matching '{exactType}', but did.";
        }

        internal static string NoExceptionInstanceMessage(Exception instance)
        {
            return $"Expected exception '{instance}', but no exception was encountered.";
        }

        internal static string NoExceptionInstanceMessage(Exception expected, Exception encountered)
        {
            return $"Expected exception '{expected}', but '{encountered}' was encountered.";
        }

        internal static string ExceptionInstanceMessage(Exception instance)
        {
            return $"Expected not to encounter exception '{instance}', but did.";
        }

        internal const string UnexpectedExceptionMessage = "Unexpected exception was encountered during an assertion.";
    }
}
