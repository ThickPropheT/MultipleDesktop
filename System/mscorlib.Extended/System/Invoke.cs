using System;

namespace System.Extended
{
    public static class Invoke
    {
        public static bool Delegate(Action @delegate, bool @if)
        {
            if (@if) @delegate();
            return @if;
        }
    }
}
