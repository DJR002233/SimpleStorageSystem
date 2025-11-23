using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels;

namespace SimpleStorageSystem.AvaloniaDesktop.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        var screen = Screens.Primary;
        if (screen != null)
        {
            const double scale = 0.4;
            var workingArea = screen.WorkingArea;
            Width = workingArea.Width * scale;
            Height = workingArea.Height * scale;
        }
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.HostScreen.Router, v => v.RoutedViewHost.Router).DisposeWith(disposables);
            // Unhandled exception. System.ArgumentException: Property 'Avalonia.Animation.IPageTransition CurrentTransition' is not defined for type 'ReactiveUI.IScreen' (Parameter 'property')
            this.OneWayBind(ViewModel, vm => ((RouterHandler)vm.HostScreen).CurrentTransition, v => v.RoutedViewHost.PageTransition).DisposeWith(disposables);
        });
    }
}