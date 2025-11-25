using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Main;

public partial class MainMenuView : ReactiveUserControl<MainMenuViewModel>
{
    public MainMenuView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.LogoutCommand, v => v.LogoutButton).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.LoadingOverlay.IsVisible, v => v.LoadingOverlay.Visible).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.LoadingOverlay.Message, v => v.LoadingOverlay.Message).DisposeWith(disposables);
        });
    }
}