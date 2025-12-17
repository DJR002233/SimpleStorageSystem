using System.Reflection;
using System.Runtime.CompilerServices;

namespace SimpleStorageSystem.Daemon.Services;

public static class News
{
    private static readonly Dictionary<Type, List<Action<object>>> _subscriptions = new ();
    public static void Publish<T>(T data)
    {
        Type key = typeof(T);
        if (data is null || !_subscriptions.TryGetValue(key, out var handlers))
            return;
        
        foreach(Action<object> handler in handlers)
        {
            MethodInfo methodInfo = handler.Method;
            // if(methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
            // {
            //     Task.Run(async () => await handler(data));
            //     continue;
            // }
            Task.Run(() => handler(data));
        }
    }

    public static void Subscribe<TEvent>(Action<TEvent> handler)
    {
        Type key = typeof(TEvent);
        if(!_subscriptions.ContainsKey(key))
            _subscriptions[key] = new List<Action<object>>();

        _subscriptions[key].Add(param => handler((TEvent)param));
    }

}
