using System;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class SettingsPageViewModel : ReactiveObject, IMainMenuPage
{
    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    #endregion VMs
    
    #region Commands
    #endregion Commands

    #region Properties
    public string Name => "Settings";
    public Type PageType => GetType();
    #endregion Properties

    public SettingsPageViewModel(
        LoadingOverlay loadingOverlay
    )
    {
        LoadingOverlay = loadingOverlay;

    }
    
}
