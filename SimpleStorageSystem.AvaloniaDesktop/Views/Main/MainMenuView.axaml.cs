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
            this.BindCommand(ViewModel, vm => vm.TestCommand, v => v.TestButton).DisposeWith(disposables);
        });
    }
}