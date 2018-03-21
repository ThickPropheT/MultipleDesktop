namespace System.Extended
{
    public delegate void ProxyEventHandler(EventHandler proxyHandler, object sender, EventArgs e);

    public static class ProxyEventHandlerExtensions
    {
        public static void BootstrapProxy(this ProxyEventHandler proxiedHandler, object sender, EventArgs e)
            => proxiedHandler.Invoke(proxiedHandler.BootstrapProxy, sender, e);
    }
}
