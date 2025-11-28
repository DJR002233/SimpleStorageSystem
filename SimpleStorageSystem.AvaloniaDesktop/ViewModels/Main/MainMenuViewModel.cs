using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

public class MainMenuViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "MainMenuView";
    private readonly INavigation Navigation;
    public IScreen HostScreen => Navigation;
    private RoutingState Router => HostScreen.Router;
    #endregion IRoutableViewModel

    #region Services
    private readonly AuthService _authService;
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    private readonly Func<LoginViewModel> _loginVM;
    #endregion VMs
    
    #region Commands
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }
    #endregion Commands

    #region Properties
    
    #endregion Properties
    public MainMenuViewModel(
        INavigation navigation,
        AuthService authService, LoadingOverlay loadingOverlay,
        Func<LoginViewModel> loginVM
    )
    {
        Navigation = navigation;
        _authService = authService;
        LoadingOverlay = loadingOverlay;
        _loginVM = loginVM;

        LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
    }

    public async Task Logout()
    {
        Response res = await _authService.LogoutAsync();
        Navigation.NavigateTo(_loginVM());
    }
    
}
