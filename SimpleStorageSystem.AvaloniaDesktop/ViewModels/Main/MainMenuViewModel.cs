using System;
using System.Reactive;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

public class MainMenuViewModel : ReactiveObject, IRoutableViewModel
{
    #region IRoutableViewModel
    public string? UrlPathSegment => "LoginView";
    public IScreen HostScreen { get; }
    private RoutingState Router => HostScreen.Router;
    #endregion IRoutableViewModel

    #region Services.Components
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services.Components

    #region VMs
    private readonly Func<LoginViewModel> _loginVM;
    #endregion VMs
    
    #region Commands
    public ReactiveCommand<Unit, Unit> TestCommand { get; }
    #endregion Commands

    public MainMenuViewModel(IScreen screen, LoadingOverlay loadingOverlay, Func<LoginViewModel> loginVM)
    {
        HostScreen = screen;
        LoadingOverlay = loadingOverlay;
        _loginVM = loginVM;

        TestCommand = ReactiveCommand.Create(Test);
    }
    public void Test()
    {
        Router.Navigate.Execute(_loginVM());
    }
}