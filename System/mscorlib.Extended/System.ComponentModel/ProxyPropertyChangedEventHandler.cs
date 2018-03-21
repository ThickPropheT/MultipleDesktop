namespace System.ComponentModel.Extended
{
    public delegate void ProxyPropertyChangedEventHandler(PropertyChangedEventHandler proxyHandler, object sender, PropertyChangedEventArgs e);

    public static class ProxyPropertyChangedEventHandlerExtensions
    {
        public static void BootstrapProxy(this ProxyPropertyChangedEventHandler proxiedHandler, object sender, PropertyChangedEventArgs e)
            => proxiedHandler.Invoke(proxiedHandler.BootstrapProxy, sender, e);
    }
}
