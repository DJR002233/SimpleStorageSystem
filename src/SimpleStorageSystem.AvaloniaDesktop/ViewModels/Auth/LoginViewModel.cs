using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using ReactiveUI;
using System.Reactive;
using System;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.AvaloniaDesktop.Client;
using Avalonia.Controls;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

public class LoginViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "LoginView";
    private readonly INavigation Navigation;
    public IScreen HostScreen => Navigation;
    #endregion IRoutableViewModel
    
    #region Services
    public AuthClient _authClient;
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    private readonly Func<MainMenuViewModel> _mainMenuVM;
    private readonly Func<CreateAccountViewModel> _createAccountVM;
    #endregion VMs

    #region Commands
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateAccountViewCommand { get; }
    #endregion Commands

    #region Properties
    public string? Email { get; set; }
    [Reactive] public string? Password { get; set; }

    #endregion Properties

    public LoginViewModel(
        INavigation screen,
        AuthClient authClient, LoadingOverlay loadingOverlay,
        Func<MainMenuViewModel> mainMenuVM, Func<CreateAccountViewModel> createAccountVM
    )
    {
        Navigation = screen;
        _authClient = authClient;
        LoadingOverlay = loadingOverlay;
        _mainMenuVM = mainMenuVM;
        _createAccountVM = createAccountVM;

        LoginCommand = ReactiveCommand.CreateFromTask(LoginButtonAsync);
        CreateAccountViewCommand = ReactiveCommand.Create(CreateAccountViewAsync);
    }

    public async Task LoginButtonAsync()
    {
        if (String.IsNullOrWhiteSpace(Email) || String.IsNullOrWhiteSpace(Password))
        {
            await DialogBox.ShowOk(ApiStatus.Failed.ToString(), "Invalid Credentials!");
            return;
        }

        IpcResponse ipcResponse = await LoadingOverlay.FromAsync( () => _authClient.RequestLoginAsync(Email, Password), "Logging in...");
        Password = "";
        
        if (ipcResponse.Status == IpcStatus.Ok)
        {
            Navigation.NavigateTo(_mainMenuVM());
            return;
        }

        await DialogBox.ShowOk(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public void CreateAccountViewAsync()
    {
        Navigation.NavigateTo(_createAccountVM());
    }
    
}
