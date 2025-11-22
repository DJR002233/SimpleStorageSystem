using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels;

namespace SimpleStorageSystem.AvaloniaDesktop.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        var screen = Screens.Primary;
        if (screen != null)
        {
            const double scale = 0.4;
            var workingArea = screen.WorkingArea;
            this.Width = workingArea.Width * scale;
            this.Height = workingArea.Height * scale;
        }
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.HostScreen.Router, v => v.RoutedViewHost.Router).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.CurrentTransition, v => v.RoutedViewHost.PageTransition).DisposeWith(disposables);
        });
    }
}