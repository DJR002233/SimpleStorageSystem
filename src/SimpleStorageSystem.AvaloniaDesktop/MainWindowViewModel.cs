using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Animation;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Client;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
{
    #region INavigation
    public INavigation Navigation { get; }
    // private RoutingState Router => Navigation.Router;
    #endregion INavigation

    public ViewModelActivator Activator { get; } = new();

    #region Events
    private readonly OnUnauthorizedHandler _authEvent;
    #endregion Events

    private readonly AuthClient _authClient;

    #region ViewModels
    private readonly LoginViewModel _loginVM;
    private readonly MainMenuViewModel _mainMenuVM;
    #endregion ViewModels
    
    public MainWindowViewModel(
        INavigation navigation,
        OnUnauthorizedHandler authEvent,
        AuthClient authClient,
        LoginViewModel loginVM, MainMenuViewModel mainMenuVM
    )
    {
        Navigation = navigation;
        _authEvent = authEvent;
        _authEvent.OnUnauthorized += HandleUnauthorized;
        _authClient = authClient;
        _loginVM = loginVM;
        _mainMenuVM = mainMenuVM;

        this.WhenActivated(disposables =>
        {
            Observable.FromAsync(Initialize).Subscribe().DisposeWith(disposables);
        });
    }

    public async Task Initialize()
    {
        await Task.Delay(1000);
        IpcResponse res = await _authClient.RequestHasSessionAsync();
        if (res.Status == IpcStatus.Ok)
        {
            Navigation.NavigateTo(_mainMenuVM, new CrossFade {Duration = TimeSpan.FromMilliseconds(600)});
            return;
        }

        Navigation.NavigateTo(_loginVM, new CrossFade {Duration = TimeSpan.FromMilliseconds(600)});
    }

    private void HandleUnauthorized()
    {
        Navigation.NavigateTo(_loginVM);
    }

}
