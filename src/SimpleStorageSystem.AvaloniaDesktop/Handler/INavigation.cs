using Avalonia.Animation;
using ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler;

public interface INavigation : IScreen
{
    IPageTransition? Transition { get; set; }
    
    void NavigateTo(IRoutableViewModel viewModel, IPageTransition? transition = null);
    // void NavigateToAsync(IRoutableViewModel viewModel, IPageTransition? transition = null);

    void NavigateBack(IPageTransition? transition = null);
    // void NavigateBackAsync(IPageTransition? transition = null);

    void NavigateAndReset(IRoutableViewModel viewModel, IPageTransition? transition = null);
    // void NavigateAndResetAsync(IRoutableViewModel viewModel, IPageTransition? transition = null);

}