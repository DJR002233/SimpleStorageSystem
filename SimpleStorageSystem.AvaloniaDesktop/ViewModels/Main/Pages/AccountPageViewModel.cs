using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class AccountPageViewModel : ReactiveObject
{
    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    #endregion VMs
    
    #region Commands
    #endregion Commands

    #region Properties
    
    #endregion Properties

    public AccountPageViewModel(
        LoadingOverlay loadingOverlay
    )
    {
        LoadingOverlay = loadingOverlay;

    }
    
}
