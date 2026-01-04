using System;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

public class MainMenuViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "MainMenuView";
    private readonly INavigation Navigation;
    public IScreen HostScreen => Navigation;
    #endregion IRoutableViewModel

    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    private readonly ActivityPageViewModel _activityPageVM;
    private readonly SettingsPageViewModel _settingsPageVM;
    private readonly StorageDrivesPageViewModel _storageDrivesPageVM;
    #endregion VMs
    
    #region Commands
    public ReactiveCommand<Unit, Unit> ShowActivityPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowSettingsPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowStorageDevicesPageCommand { get; }
    #endregion Commands

    #region Properties
    [Reactive] public string? PageTitle { get; set; }
    [Reactive] public ReactiveObject? CurrentPage { get; set; }
    #endregion Properties
    
    public MainMenuViewModel(
        INavigation navigation, LoadingOverlay loadingOverlay,
        ActivityPageViewModel activityPageVM, SettingsPageViewModel settingsPageVM, StorageDrivesPageViewModel storageDrivesPageVM
    )
    {
        Navigation = navigation;

        LoadingOverlay = loadingOverlay;

        _activityPageVM = activityPageVM;
        _settingsPageVM = settingsPageVM;
        _storageDrivesPageVM = storageDrivesPageVM;

        NavigateToActivityPage();

        ShowActivityPageCommand = ReactiveCommand.Create(NavigateToActivityPage);
        ShowSettingsPageCommand = ReactiveCommand.Create(NavigateToSettingsPage);
        ShowStorageDevicesPageCommand = ReactiveCommand.Create(NavigateToStorageDrivesPage);
    }

    public void NavigateToActivityPage()
    {
        CurrentPage = _activityPageVM;
        PageTitle = "Activity";
    }

    public void NavigateToSettingsPage()
    {
        CurrentPage = _settingsPageVM;
        PageTitle = "Settings";
    }

    public void NavigateToStorageDrivesPage()
    {
        CurrentPage = _storageDrivesPageVM;
        PageTitle = "Storage Drives";
    }

    public void StorageDriveInformationView(Guid id)
    {
        // Navigation.NavigateTo(_viewContainer(vm));
    }
    
}
