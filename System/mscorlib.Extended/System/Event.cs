namespace System.Extended
{
    public static class Event
    {
        public delegate void Subscriber(EventHandler handler);
        public delegate void Subscriber<T>(T target, EventHandler handler);

        public static void HandleOnce<T>(this T t, Subscriber subscribe, Subscriber unsubscribe, EventHandler doHandle)
            => HandleOnce(subscribe, unsubscribe, doHandle);

        public static void HandleOnce<T>(this T t, Subscriber<T> subscribe, Subscriber<T> unsubscribe, EventHandler doHandle)
            => HandleOnce(
                e => subscribe(t, e),
                e => unsubscribe(t, e),
                doHandle);

        public static void HandleOnce(Subscriber subscribe, Subscriber unsubscribe, EventHandler doHandle)
        {
            subscribe(
                new UnsubscribingHandler(
                    (thisHandler, s, e) =>
                    {
                        unsubscribe(thisHandler);
                        doHandle(s, e);
                    })
                .UnsubscribeThenInvoke);
        }

        private delegate void UnsubscribingHandler(EventHandler invokedHandler, object sender, EventArgs e);

        private static void UnsubscribeThenInvoke(this UnsubscribingHandler unsubscribingHandler, object sender, EventArgs e)
        {
            unsubscribingHandler(unsubscribingHandler.UnsubscribeThenInvoke, sender, e);
        }
    }
}
