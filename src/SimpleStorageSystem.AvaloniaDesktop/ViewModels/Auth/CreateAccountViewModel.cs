using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Client;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

public class CreateAccountViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "CreateAccountView";
    private readonly INavigation Navigation;
    public IScreen HostScreen => Navigation;
    #endregion IRoutableViewModel

    #region Services
    private readonly AuthClient _authClient;
    public LoadingOverlay LoadingOverlay { get; }
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
    [Reactive] public string? Password { get; set; }
    [Reactive] public string? RePassword { get; set; }
    #endregion Properties

    public CreateAccountViewModel(
        INavigation navigation,
        AuthClient authClient, LoadingOverlay loadingOverlay,
        Func<LoginViewModel> loginVM
    )
    {
        Navigation = navigation;
        _authClient = authClient;
        LoadingOverlay = loadingOverlay;
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
            await DialogBox.Show(ApiStatus.Failed.ToString(), emptyTextBox);
            return;
        }

        LoadingOverlay.Show("Creating account...");
        IpcResponse ipcResponse = await _authClient.RequestCreateAccountAsync(Username!, Email!, Password!);
        LoadingOverlay.Close();
        RePassword = "";
        
        if (ipcResponse.Status == IpcStatus.Ok)
        {
            Password = "";
            await DialogBox.Show("Account created successfully!", 
                "Click Ok to redirect to the login page...");
            Navigation.NavigateTo(_loginVM());
            return;
        }

        await DialogBox.Show(ipcResponse.Status.ToString(), ipcResponse.Payload);
    }

    public void Back()
    {
        Navigation.NavigateBack();
    }
    
}