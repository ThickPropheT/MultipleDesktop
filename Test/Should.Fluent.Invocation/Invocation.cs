﻿using System;

namespace Should.Fluent.Invocation
{
    public static class Invocation
    {
        public static Invoker Of(Action code)
        {
            return new Invoker(code);
        }

        public static Invoker Of(Func<object> code)
        {
            return new Invoker(code);
        }

        public struct Result
        {
            public bool DidSucceed { get; private set; }
            public object ValueReturned { get; private set; }
            public Exception Error { get; private set; }

            public static Result Succeeded()
            {
                return new Result
                {
                    DidSucceed = true,
                    ValueReturned = null,
                    Error = null
                };
            }

            public static Result Succeeded(object result)
            {
                return new Result
                {
                    DidSucceed = true,
                    ValueReturned = result,
                    Error = null
                };
            }

            public static Result FailedWith(Exception error)
            {
                return new Result
                {
                    DidSucceed = false,
                    ValueReturned = null,
                    Error = error
                };
            }
        }
    }
}
