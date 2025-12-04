using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Main.Pages;

public partial class SettingsPageView : ReactiveUserControl<SettingsPageViewModel>
{
    public SettingsPageView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // this.BindCommand(ViewModel, vm => vm.ToggleMenuCommand, v => v.SettingsButton).DisposeWith(disposables);
        });

    }

}
