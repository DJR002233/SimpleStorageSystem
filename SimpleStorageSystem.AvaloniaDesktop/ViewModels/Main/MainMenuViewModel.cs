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
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

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
    private readonly AccountPageViewModel _accountPageVM;
    private readonly ActivityPageViewModel _activityPageVM;
    private readonly SettingsPageViewModel _settingsPageVM;
    private readonly StorageDevicesPageViewModel _storageDevicesPageVM;
    private readonly Func<LoginViewModel> _loginVM;
    #endregion VMs
    
    #region Commands
    public ReactiveCommand<Unit, Unit> ShowAccountPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowActivityPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowSettingsPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowStorageDevicesPageCommand { get; }
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }
    #endregion Commands

    #region Properties
    [Reactive] public ReactiveObject CurrentPage { get; set; }
    #endregion Properties
    
    public MainMenuViewModel(
        INavigation navigation,
        AuthService authService, LoadingOverlay loadingOverlay,
        AccountPageViewModel accountPageVM, ActivityPageViewModel activityPageVM, SettingsPageViewModel settingsPageVM, StorageDevicesPageViewModel storageDevicesPageVM, Func<LoginViewModel> loginVM
    )
    {
        Navigation = navigation;

        _authService = authService;
        LoadingOverlay = loadingOverlay;

        _accountPageVM = accountPageVM;
        _activityPageVM = activityPageVM;
        _settingsPageVM = settingsPageVM;
        _storageDevicesPageVM = storageDevicesPageVM;
        _loginVM = loginVM;

        CurrentPage = _activityPageVM;

        ShowAccountPageCommand = ReactiveCommand.Create(NavigateToAccountPage);
        ShowActivityPageCommand = ReactiveCommand.Create(NavigateToActivityPage);
        ShowSettingsPageCommand = ReactiveCommand.Create(NavigateToSettingsPage);
        ShowStorageDevicesPageCommand = ReactiveCommand.Create(NavigateToStorageDevicesPage);
        LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
    }

    public void NavigateToAccountPage()
    {
        CurrentPage = _accountPageVM;
    }

    public void NavigateToActivityPage()
    {
        CurrentPage = _activityPageVM;
    }

    public void NavigateToSettingsPage()
    {
        CurrentPage = _settingsPageVM;
    }

    public void NavigateToStorageDevicesPage()
    {
        CurrentPage = _storageDevicesPageVM;
    }

    public async Task Logout()
    {
        Response res = await _authService.LogoutAsync();
        Navigation.NavigateTo(_loginVM());
    }
    
}
