using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Client;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

public class MainMenuViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "MainMenuView";
    private readonly INavigation Navigation;
    public IScreen HostScreen => Navigation;
    #endregion IRoutableViewModel

    #region Services
    private readonly AuthClient _authClient;
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    private readonly AccountPageViewModel _accountPageVM;
    private readonly ActivityPageViewModel _activityPageVM;
    private readonly SettingsPageViewModel _settingsPageVM;
    private readonly StorageDrivesPageViewModel _storageDrivesPageVM;
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
    [Reactive] public string? PageTitle { get; set; }
    [Reactive] public ReactiveObject? CurrentPage { get; set; }
    #endregion Properties
    
    public MainMenuViewModel(
        INavigation navigation,
        AuthClient authClient, LoadingOverlay loadingOverlay,
        AccountPageViewModel accountPageVM, ActivityPageViewModel activityPageVM, SettingsPageViewModel settingsPageVM, StorageDrivesPageViewModel storageDrivesPageVM, Func<LoginViewModel> loginVM
    )
    {
        Navigation = navigation;

        _authClient = authClient;
        LoadingOverlay = loadingOverlay;

        _accountPageVM = accountPageVM;
        _activityPageVM = activityPageVM;
        _settingsPageVM = settingsPageVM;
        _storageDrivesPageVM = storageDrivesPageVM;
        _loginVM = loginVM;

        NavigateToActivityPage();

        ShowAccountPageCommand = ReactiveCommand.Create(NavigateToAccountPage);
        ShowActivityPageCommand = ReactiveCommand.Create(NavigateToActivityPage);
        ShowSettingsPageCommand = ReactiveCommand.Create(NavigateToSettingsPage);
        ShowStorageDevicesPageCommand = ReactiveCommand.Create(NavigateToStorageDrivesPage);
        LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
    }

    public void NavigateToAccountPage()
    {
        CurrentPage = _accountPageVM;
        PageTitle = "Account";
    }

    public void NavigateToActivityPage()
    {
        CurrentPage = _activityPageVM;
        PageTitle = "Activity";
    }

    public void NavigateToSettingsPage()
    {
        CurrentPage = _settingsPageVM;
        PageTitle = "Settings";
    }

    public void NavigateToStorageDrivesPage()
    {
        CurrentPage = _storageDrivesPageVM;
        PageTitle = "Storage Drives";
    }

    public async Task Logout()
    {
        IpcResponse ipcResponse = await _authClient.RequestLogoutAsync();

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            Navigation.NavigateTo(_loginVM());
            return;
        }
        
        await DialogBox.Show(ipcResponse.Status.ToString(), ipcResponse.Message);
    }
    
}
