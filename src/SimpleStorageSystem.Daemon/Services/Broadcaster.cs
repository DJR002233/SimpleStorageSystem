namespace SimpleStorageSystem.Daemon.Services;

public static class Broadcaster
{
    private static readonly object _lock = new();
    private static readonly Dictionary<Type, List<SubscriberHandler<object>>> _subscriptions = new();
    public static async ValueTask PublishInOrder<T>(T data)
    {
        if (data is null)
            return;

        Type key = typeof(T);

        if (!_subscriptions.TryGetValue(key, out var handlers))
            return;

        foreach (SubscriberHandler<object> handler in handlers)
        {
            await handler(data);
        }
    }

    public static void Subscribe<TEvent>(Func<TEvent, ValueTask> handler)
    {
        lock (_lock)
        {
            Type key = typeof(TEvent);
            if (!_subscriptions.ContainsKey(key))
                _subscriptions[key] = new List<SubscriberHandler<object>>();

            _subscriptions[key].Add(param => handler((TEvent)param));
        }
    }

    public delegate ValueTask SubscriberHandler<in TEvent>(TEvent data);

}
