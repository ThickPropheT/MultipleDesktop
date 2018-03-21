using System.ComponentModel;

namespace System.ComponentModel.Extended
{
    public static class Event
    {
        public delegate void Subscriber(PropertyChangedEventHandler handler);
        public delegate void Subscriber<T>(T target, PropertyChangedEventHandler handler);
        public delegate bool Handler(object sender, PropertyChangedEventArgs e);

        public static PropertyChangedEventHandler HandleFirstChange(Subscriber unsubscribe, Handler doHandle)
            => new ProxyPropertyChangedEventHandler(
                (thisHandler, s, e) =>
                {
                    if (doHandle(s, e))
                        unsubscribe(thisHandler);
                })
            .BootstrapProxy;
    }
}
