using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;
using ReactiveUI;
using System.Reactive;
using System;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

public class LoginViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "LoginView";
    public IScreen HostScreen { get; }
    private RoutingState Router => HostScreen.Router;
    #endregion IRoutableViewModel
    
    #region Services.Auth
    public AuthService _authService;
    #endregion Services.Auth

    #region Services.Components
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services.Components

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
    public string? Password { get; set; }

    #endregion Properties

    public LoginViewModel(
        IScreen screen, LoadingOverlay loadingOverlay, AuthService authService, 
        Func<MainMenuViewModel> mainMenuVM, Func<CreateAccountViewModel> createAccountVM
    )
    {
        HostScreen = screen;
        LoadingOverlay = loadingOverlay;
        _authService = authService;
        _createAccountVM = createAccountVM;
        _mainMenuVM = mainMenuVM;

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

        Response res = await _authService.LoginAsync(Email, Password);
        
        if (res.StatusMessage == StatusMessage.Success)
        {
            Router.Navigate.Execute(_mainMenuVM());
            return;
        }

        await DialogBox.Show(res.Title, res.Message);
    }

    public void CreateAccountViewAsync()
    {
        Router.Navigate.Execute(_createAccountVM());
    }

    //public async Task InitializeAsync() {}

}