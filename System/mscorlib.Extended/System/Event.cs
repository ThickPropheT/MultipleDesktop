namespace System.Extended
{
    public static class Event
    {
        public delegate void Subscriber(EventHandler handler);
        public delegate void Subscriber<T>(T target, EventHandler handler);

        public static EventHandler HandleOnce(Subscriber unsubscribe, EventHandler doHandle)
            => new ProxyEventHandler(
                (thisHandler, s, e) =>
                {
                    unsubscribe(thisHandler);
                    doHandle(s, e);
                })
            .BootstrapProxy;
    }
}
