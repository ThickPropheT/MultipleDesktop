using System.ComponentModel;

namespace System.ComponentModel.Extended
{
    public static class Event
    {
        public delegate void Subscriber(PropertyChangedEventHandler handler);
        public delegate void Subscriber<T>(T target, PropertyChangedEventHandler handler);
        public delegate bool Handler(object sender, PropertyChangedEventArgs e);

        public static void HandleOnce<T>(this T t, Subscriber subscribe, Subscriber unsubscribe, Handler doHandle)
            where T : INotifyPropertyChanged
            => HandleOnce(subscribe, unsubscribe, doHandle);

        public static void HandleOnce<T>(this T t, Subscriber<T> subscribe, Subscriber<T> unsubscribe, Handler doHandle)
            where T : INotifyPropertyChanged
            => HandleOnce(
                e => subscribe(t, e),
                e => unsubscribe(t, e),
                doHandle);

        public static void HandleOnce(Subscriber subscribe, Subscriber unsubscribe, Handler doHandle)
        {
            subscribe(
                new UnsubscribingHandler(
                    (thisHandler, s, e) =>
                    {
                        if (doHandle(s, e))
                            unsubscribe(thisHandler);
                    })
                .UnsubscribeThenInvoke);
        }

        private delegate void UnsubscribingHandler(PropertyChangedEventHandler invokedHandler, object sender, PropertyChangedEventArgs e);

        private static void UnsubscribeThenInvoke(this UnsubscribingHandler unsubscribingHandler, object sender, PropertyChangedEventArgs e)
        {
            unsubscribingHandler(unsubscribingHandler.UnsubscribeThenInvoke, sender, e);
        }
    }
}
