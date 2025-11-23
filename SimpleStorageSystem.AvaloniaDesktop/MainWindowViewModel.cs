using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Animation;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
{
    #region IScreen
    public IScreen HostScreen { get; }
    private RoutingState Router => HostScreen.Router;
    #endregion IScreen

    #region Others
    // private IPageTransition? _currentTransition;
    // public IPageTransition? CurrentTransition
    // {
    //     get => _currentTransition;
    //     set => this.RaiseAndSetIfChanged(ref _currentTransition, value);
    // }
    public ViewModelActivator Activator { get; } = new();
    #endregion Others

    #region Events
    private readonly OnUnauthorizedHandler _authEvent;
    #endregion Events

    private readonly AuthService _authService;
    private readonly LoginViewModel _loginVM;
    
    public MainWindowViewModel(
        IScreen screen, OnUnauthorizedHandler authEvent, AuthService authService,
        LoginViewModel loginVM
    )
    {
        HostScreen = screen;
        _authEvent = authEvent;
        _authService = authService;
        _loginVM = loginVM;

        ((RouterHandler)HostScreen).CurrentTransition = null;

        _authEvent.OnUnauthorized += HandleUnauthorized;

        this.WhenActivated(disposables =>
        {
            ((RouterHandler)HostScreen).CurrentTransition = new CrossFade {Duration = TimeSpan.FromMilliseconds(600)};
            Observable.FromAsync(Initialize).Subscribe().DisposeWith(disposables);
        });
    }

    public async Task Initialize()
    {
        await Task.Delay(3000);
        await Router.Navigate.Execute(_loginVM);
    }

    private void HandleUnauthorized()
    {
        Router.Navigate.Execute(_loginVM);
    }

}
