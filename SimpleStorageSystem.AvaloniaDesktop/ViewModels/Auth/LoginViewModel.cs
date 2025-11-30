using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;
using ReactiveUI;
using System.Reactive;
using System;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using ReactiveUI.Fody.Helpers;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

public class LoginViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "LoginView";
    private readonly INavigation Navigation;
    public IScreen HostScreen => Navigation;
    #endregion IRoutableViewModel
    
    #region Services
    public AuthService _authService;
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
        AuthService authService, LoadingOverlay loadingOverlay,
        Func<MainMenuViewModel> mainMenuVM, Func<CreateAccountViewModel> createAccountVM
    )
    {
        Navigation = screen;
        _authService = authService;
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
            await DialogBox.Show(StatusMessage.Failed.ToString(), "Invalid Credentials!");
            return;
        }

        LoadingOverlay.Show("Logging in...");
        Response res = await _authService.LoginAsync(Email, Password);
        LoadingOverlay.Close();
        Password = "";
        
        if (res.StatusMessage == StatusMessage.Success)
        {
            Navigation.NavigateTo(_mainMenuVM());
            return;
        }

        await DialogBox.Show(res.Title, res.Message);
    }

    public void CreateAccountViewAsync()
    {
        Navigation.NavigateTo(_createAccountVM());
    }
    
}