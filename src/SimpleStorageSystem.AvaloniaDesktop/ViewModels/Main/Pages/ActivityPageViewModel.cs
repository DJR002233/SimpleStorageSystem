using System;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class ActivityPageViewModel : ReactiveObject, IMainMenuPage
{
    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    #endregion VMs
    
    #region Commands
    // public ReactiveCommand<Unit, Unit> ShowLoading;
    #endregion Commands

    #region Properties
    public string Name => "Activity";
    public Type PageType => GetType();
    #endregion Properties

    public ActivityPageViewModel(
        LoadingOverlay loadingOverlay
    )
    {
        LoadingOverlay = loadingOverlay;

        // ShowLoading = ReactiveCommand.Create(ShowLoadingOverlay);
    }

    // public void ShowLoadingOverlay()
    // {
    //     LoadingOverlay.Show("Loading from ActivityPageViewModel");
    // }
    
}
