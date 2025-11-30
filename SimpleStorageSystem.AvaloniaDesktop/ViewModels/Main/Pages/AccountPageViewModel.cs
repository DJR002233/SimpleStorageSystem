using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;
using SimpleStorageSystem.AvaloniaDesktop.Services.Main;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class AccountPageViewModel : ReactiveObject
{
    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    private readonly AccountService _accountService;
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
        AccountService accountService
    )
    {
        LoadingOverlay = loadingOverlay;
        _accountService = accountService;

        SaveCommand = ReactiveCommand.CreateFromTask(Save);
    }

    public async Task Save()
    {
        if(!String.IsNullOrWhiteSpace(Password) && !String.Equals(Password, RePassword))
        {
            await DialogBox.Show("Warning","Password does not match!");
            return;
        }
        
        LoadingOverlay.Show("Updating Account Information...");
        Response res = await _accountService.UpdateAccountInformation(Username, Email, Password);
        LoadingOverlay.Close();
        RePassword = "";

        if (res.StatusMessage == StatusMessage.Success)
        {
            Password = "";
            await DialogBox.Show(res.StatusMessage.ToString(), "Account Updated!");
            return;
        }

        await DialogBox.Show(res.Title, res.Message);
    }
    
}
