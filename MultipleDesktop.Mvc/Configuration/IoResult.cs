using System;
using System.IO;

namespace MultipleDesktop.Mvc.Configuration
{
    public struct IoResult
    {
        public bool DidFail { get; private set; }
        public Exception Exception { get; private set; }

        public bool? DoesExist { get; private set; }
        public bool? ReadError { get; private set; }

        public static IoResult ForSuccess()
            => new IoResult
            {
                DidFail = false,
                Exception = null,
                DoesExist = true,
                ReadError = false
            };

        public static IoResult ForReadError(InvalidOperationException exception)
            => new IoResult
            {
                DidFail = true,
                Exception = exception,
                DoesExist = null,
                ReadError = true
            };

        public static IoResult ForNotFound(IOException exception)
            => new IoResult
            {
                DidFail = true,
                Exception = exception,
                DoesExist = false,
                ReadError = null
            };

        public static IoResult ForException(Exception exception)
            => new IoResult
            {
                DidFail = true,
                Exception = exception,
                DoesExist = null,
                ReadError = null
            };
    }
}
