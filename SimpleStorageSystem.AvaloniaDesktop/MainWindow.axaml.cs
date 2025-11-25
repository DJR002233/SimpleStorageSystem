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
            this.Bind(ViewModel, vm => vm.Navigation.Router, v => v.RoutedViewHost.Router).DisposeWith(disposables);
            this.OneWayBind(ViewModel, vm => vm.Navigation.Transition, v => v.RoutedViewHost.PageTransition).DisposeWith(disposables);
            // ViewModel!.HostScreen.Transition = new CrossFade {Duration = TimeSpan.FromMilliseconds(600)};
            // Observable.FromAsync(ViewModel.Initialize).Subscribe().DisposeWith(disposables);
        });
    }
}