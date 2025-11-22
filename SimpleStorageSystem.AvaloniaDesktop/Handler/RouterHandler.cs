using ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler;

public class RouterHandler : IScreen
{
    public RoutingState Router { get; } = new RoutingState();
}