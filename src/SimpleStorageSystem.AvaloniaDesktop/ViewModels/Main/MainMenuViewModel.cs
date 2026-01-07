using System;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.PageFactory;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

public class MainMenuViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "MainMenuView";
    private readonly INavigation Navigation;
    public IScreen HostScreen => Navigation;
    #endregion IRoutableViewModel

    public ViewModelActivator Activator { get; } = new();

    #region Services
    private readonly DialogBox _dialogBox;
    public LoadingOverlay LoadingOverlay { get; }
    private readonly IPageFactory<IMainMenuPage> _mainMenuPageFactory;
    #endregion Services

    #region VMs
    public Type ActivityPage => typeof(ActivityPageViewModel);
    public Type SettingsPage => typeof(SettingsPageViewModel);
    public Type StorageDrivePage => typeof(StorageDrivesPageViewModel);
    #endregion VMs

    #region Commands
    public ReactiveCommand<Type, Unit> ShowPageCommand { get; }
    #endregion Commands

    #region Properties
    [Reactive] public string? PageTitle { get; set; }
    [Reactive] public ReactiveObject? CurrentPage { get; set; }
    #endregion Properties

    public MainMenuViewModel(
        INavigation navigation,
        LoadingOverlay loadingOverlay, DialogBox dialogBox,
        IPageFactory<IMainMenuPage> mainMenuPageFactory
    )
    {
        Navigation = navigation;

        LoadingOverlay = loadingOverlay;
        _dialogBox = dialogBox;

        _mainMenuPageFactory = mainMenuPageFactory;

        ShowPageCommand = ReactiveCommand.CreateFromTask<Type>(NavigateToPage);

        this.WhenActivated(disposables =>
        {
            Observable.FromAsync(_ => NavigateToPage(typeof(StorageDrivesPageViewModel))).Subscribe().DisposeWith(disposables);
        });
    }

    public async Task NavigateToPage(Type pageType)
    {
        try
        {
            var mainMenuPage = _mainMenuPageFactory.Create(pageType);

            CurrentPage = (ReactiveObject)mainMenuPage;

            PageTitle = mainMenuPage.Name;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _dialogBox.ShowOkOnly("Page not found");
        }
    }

    public void StorageDriveInformationView(Guid id)
    {
        // Navigation.NavigateTo(_viewContainer(vm));
    }

}
