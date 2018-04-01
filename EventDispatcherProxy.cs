namespace MyNamespace.Utils
{
    public interface IEventDispatcherProxy
    {
        EventDispatcher Dispatcher { get; }
    }

    public static class EventDispatcherProxyExtension
    {
        public static void SendEvent(this IEventDispatcherProxy proxy, short type)
        {
            proxy.Dispatcher.SendEvent(type);
        }

        public static void SendEvent<T>(this IEventDispatcherProxy proxy, short type, T msg)
        {
            proxy.Dispatcher.SendEvent<T>(type, msg);
        }

        public static void AddEventHandler(this IEventDispatcherProxy proxy, short type, EventHandler handler)
        {
            proxy.Dispatcher.AddEventHandler(type, handler);
        }

        public static void AddEventHandler<T>(this IEventDispatcherProxy proxy, short type, EventHandler<T> handler)
        {
            proxy.Dispatcher.AddEventHandler<T>(type, handler);
        }

        public static void RemoveEventHandler(this IEventDispatcherProxy proxy, short type, EventHandler handler)
        {
            proxy.Dispatcher.RemoveEventHandler(type, handler);
        }

        public static void RemoveEventHandler<T>(this IEventDispatcherProxy proxy, short type,
            EventHandler<T> handler)
        {
            proxy.Dispatcher.RemoveEventHandler<T>(type, handler);
        }

        public static void AddOneShotEventHandler(this IEventDispatcherProxy proxy, short type, EventHandler handler)
        {
            proxy.Dispatcher.AddOneShotEventHandler(type, handler);
        }

        public static void RemoveOneShotEventHandler(this IEventDispatcherProxy proxy, short type,
            EventHandler handler)
        {
            proxy.Dispatcher.RemoveOneShotEventHandler(type, handler);
        }

        public static void AddOneShotEventHandler<T>(this IEventDispatcherProxy proxy, short type,
            EventHandler<T> handler)
        {
            proxy.Dispatcher.AddOneShotEventHandler<T>(type, handler);
        }

        public static void RemoveOneShotEventHandler<T>(this IEventDispatcherProxy proxy, short type,
            EventHandler<T> handler)
        {
            proxy.Dispatcher.RemoveOneShotEventHandler<T>(type, handler);
        }
    }
}