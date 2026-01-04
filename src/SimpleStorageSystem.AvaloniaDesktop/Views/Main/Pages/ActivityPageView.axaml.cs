using ReactiveUI;
using ReactiveUI.Avalonia;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Main.Pages;

public partial class ActivityPageView : ReactiveUserControl<ActivityPageViewModel>
{
    public ActivityPageView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // this.BindCommand(ViewModel, vm => vm.ShowLoading, v => v.ShowLoading).DisposeWith(disposables);

            // this.Bind(ViewModel, vm => vm.LoadingOverlay.IsVisible, v => v.LoadingOverlay.Visible).DisposeWith(disposables);
            // this.Bind(ViewModel, vm => vm.LoadingOverlay.Message, v => v.LoadingOverlay.Message).DisposeWith(disposables);
        });

    }

}
