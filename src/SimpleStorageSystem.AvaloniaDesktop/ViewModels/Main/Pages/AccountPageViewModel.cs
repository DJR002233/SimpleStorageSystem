using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Client.Main;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class AccountPageViewModel : ReactiveObject
{
    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    private readonly AccountClient _accountClient;
    #endregion Services
    
    #region Commands
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    #endregion Commands

    #region Properties
    public string? Username { get; set; }
    [Reactive] public string? Email { get; set; }
    [Reactive] public string? Password { get; set; }
    [Reactive] public string? RePassword { get; set; }
    #endregion Properties

    public AccountPageViewModel(
        LoadingOverlay loadingOverlay,
        AccountClient accountClient
    )
    {
        LoadingOverlay = loadingOverlay;
        _accountClient = accountClient;

        SaveCommand = ReactiveCommand.CreateFromTask(Save);
    }

    public async Task Save()
    {
        if (!String.IsNullOrWhiteSpace(Password) && !String.Equals(Password, RePassword))
        {
            await DialogBox.Show("Warning", "Password does not match!\n\nClear password textbox to not change it");
            return;
        }

        LoadingOverlay.Show("Delaying Task for 3 sec.");
        IpcResponse res = await _accountClient.RequestUpdateAccountInformation(Username, Email, Password);
        LoadingOverlay.Close();

        if (res.Status == IpcStatus.Ok)
        {
            await DialogBox.Show(res.Status.ToString(), "Information updated successfully!");
            return;
        }

        await DialogBox.Show(res.Status.ToString(), res.Payload);
    }
    
}
