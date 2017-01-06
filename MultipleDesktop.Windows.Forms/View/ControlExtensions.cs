using System;
using System.Windows.Forms;

namespace MultipleDesktop.Windows.Forms.View
{
    public static class ControlExtensions
    {
        public static void InvokeIfRequired<TControl>(this TControl c, Action<TControl> callback) where TControl : Control
        {
            if (c.InvokeRequired)
            {
                var invoker = new MethodInvoker(() => callback(c));
                c.Invoke(invoker);
            }
            else
            {
                callback(c);
            }
        }
    }
}
