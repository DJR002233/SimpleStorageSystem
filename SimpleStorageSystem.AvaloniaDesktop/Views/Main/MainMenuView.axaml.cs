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
            this.BindCommand(ViewModel, vm => vm.ShowAccountPageCommand, v => v.AccountMenuItem).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.ShowActivityPageCommand, v => v.ActivityMenuItem).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.ShowSettingsPageCommand, v => v.SettingsMenuItem).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.ShowStorageDevicesPageCommand, v => v.StorageDevicesMenuItem).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.LogoutCommand, v => v.LogoutMenuItem).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.CurrentPage, v => v.PageView.Content).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.LoadingOverlay.IsVisible, v => v.LoadingOverlay.Visible).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.LoadingOverlay.Message, v => v.LoadingOverlay.Message).DisposeWith(disposables);
        });

    }

}
