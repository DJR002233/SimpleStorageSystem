using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

public class CreateAccountViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "CreateAccountView";
    public IScreen HostScreen { get; }
    private RoutingState Router => HostScreen.Router;
    #endregion IRoutableViewModel

    #region Services.Components
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services.Components

    #region Services
    private readonly AuthService _authService;
    #endregion Services

    #region VMs
    private readonly Func<LoginViewModel> _loginVM;
    #endregion VMs

    #region Commands
    public ReactiveCommand<Unit, Unit> CreateAccountCommand { get; }
    public ReactiveCommand<Unit, Unit> BackCommand { get; }
    #endregion Commands

    #region Properties
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? RePassword { get; set; }
    #endregion Properties

    public CreateAccountViewModel(
        IScreen screen, LoadingOverlay loadingOverlay, AuthService authService,
        Func<LoginViewModel> loginVM
    )
    {
        HostScreen = screen;
        LoadingOverlay = loadingOverlay;
        _authService = authService;
        _loginVM = loginVM;

        CreateAccountCommand = ReactiveCommand.CreateFromTask(CreateAccountAsync);
        BackCommand = ReactiveCommand.Create(Back);
    }

    public async Task CreateAccountAsync()
    {
        string emptyTextBox = "";

        if (String.IsNullOrWhiteSpace(Username))
            emptyTextBox += "Username is required!\n";
        if (String.IsNullOrWhiteSpace(Email))
            emptyTextBox += "Email is required!\n";
        if (String.IsNullOrWhiteSpace(Password))
            emptyTextBox += "Password is required!\n";
        if (!String.Equals(Password, RePassword))
            emptyTextBox += "Retry Password does not match!";

        if (!String.IsNullOrWhiteSpace(emptyTextBox))
        {
            await DialogBox.Show(StatusMessage.Failed.ToString(), emptyTextBox);
            return;
        }

        LoadingOverlay.Show("Creating account...");
        Response res = await _authService.CreateAccountAsync(Username!, Email!, Password!);
        if (res.StatusMessage == StatusMessage.Success)
        {
            LoadingOverlay.Close();
            await DialogBox.Show(res.StatusMessage.ToString(), 
                "Account created successfully!\n\nClick Ok to automatically redirect to the login page...");
            Router.Navigate.Execute(_loginVM());
            return;
        }

        LoadingOverlay.Close();
        await DialogBox.Show(res.StatusMessage.ToString(), res.Message);
    }

    public void Back()
    {
        Router.Navigate.Execute(_loginVM());
    }
    
}