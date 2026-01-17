using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Animation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels;

public class MainWindowViewModel : ReactiveObject, IActivatableViewModel, IOverlay
{
    #region INavigation
    public INavigation Navigation { get; }
    #endregion INavigation

    public ViewModelActivator Activator { get; } = new();
    [Reactive] public ReactiveObject? OverlayViewHost { get; set; } = null;

    #region ViewModels
    private readonly Func<MainMenuViewModel> _mainMenuVM;
    #endregion ViewModels
    
    public MainWindowViewModel(
        INavigation navigation,
        Func<MainMenuViewModel> mainMenuVM
    )
    {
        Navigation = navigation;

        _mainMenuVM = mainMenuVM;

        // might be removed
        this.WhenActivated(disposables =>
        {
            Observable.FromAsync(Initialize).Subscribe().DisposeWith(disposables);
        });
    }

    // will be removed in the future
    public async Task Initialize()
    {
        await Task.Delay(1000);
        Navigation.NavigateTo(_mainMenuVM(), new CrossFade {Duration = TimeSpan.FromMilliseconds(600)});
    }

}
