using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
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
        LoadingOverlay.Show("Delaying Task for 3 sec.");
        await Task.Delay(3000);
        LoadingOverlay.Close();
    }
    
}
