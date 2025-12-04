using Avalonia.Animation;
using ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler;

public class RouterHandler : ReactiveObject, INavigation
{
    public RoutingState Router { get; } = new RoutingState();

    #region IPageTransition
    private IPageTransition? _transition;
    public IPageTransition? Transition
    {
        get => _transition;
        set => this.RaiseAndSetIfChanged(ref _transition, value);
    }
    #endregion IPageTransition

    public void NavigateTo(IRoutableViewModel viewModel, IPageTransition? transition = null)
    {
        Transition = transition;
        Router.Navigate.Execute(viewModel);
    }

    public void NavigateBack(IPageTransition? transition = null)
    {
        Transition = transition;
        Router.NavigateBack.Execute();
    }

    public void NavigateAndReset(IRoutableViewModel viewModel, IPageTransition? transition = null)
    {
        Transition = transition;
        Router.NavigateBack.Execute();
    }

}
