using Avalonia.Animation;
using ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler;

public class RouterHandler : IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    public IPageTransition? CurrentTransition { get; set; }
}