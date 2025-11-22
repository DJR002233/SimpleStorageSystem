using Avalonia.Animation;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    #region IScreen
    public IScreen HostScreen { get; }
    private RoutingState Router => HostScreen.Router;
    #endregion IScreen
    public IPageTransition? CurrentTransition { get; } = null;
    private readonly LoginViewModel _loginVM;
    private readonly OnUnauthorizedHandler _authEvent;
    
    public MainWindowViewModel(IScreen screen, OnUnauthorizedHandler authEvent, LoginViewModel loginVM)
    {
        _loginVM = loginVM;
        HostScreen = screen;
        _authEvent = authEvent;
        _authEvent.OnUnauthorized += HandleUnauthorized;

        Router.Navigate.Execute(_loginVM);
    }
    private void HandleUnauthorized()
    {
        Router.Navigate.Execute(_loginVM);
    }
}