namespace SimpleStorageSystem.Daemon.Services;

public static class Broadcaster
{
    private static readonly object _lock = new();
    private static readonly Dictionary<Type, List<SubscriberHandler<object>>> _subscribers = new();
    
    public static async ValueTask PublishInOrderAsync<T>(T data)
    {
        if (data is null)
            return;

        Type key = typeof(T);

        if (!_subscribers.TryGetValue(key, out var handlers))
            return;

        foreach (SubscriberHandler<object> handler in handlers)
        {
            await handler(data);
        }
    }

    public static async ValueTask PublishInParallelAsync<T>(T data)
    {
        if (data is null)
            return;

        Type key = typeof(T);

        if (!_subscribers.TryGetValue(key, out var handlers))
            return;

        var tasks = handlers.Select(handler => handler(data).AsTask());
        await Task.WhenAll(tasks);
    }

    public static void Subscribe<TEvent>(Func<TEvent, ValueTask> handler)
    {
        lock (_lock)
        {
            Type key = typeof(TEvent);
            if (!_subscribers.ContainsKey(key))
                _subscribers[key] = new List<SubscriberHandler<object>>();

            _subscribers[key].Add(param => handler((TEvent)param));
        }
    }

    public delegate ValueTask SubscriberHandler<in TEvent>(TEvent data);

}
