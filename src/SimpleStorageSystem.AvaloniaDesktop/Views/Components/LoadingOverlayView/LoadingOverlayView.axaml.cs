using ReactiveUI;
using ReactiveUI.Avalonia;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Components;

public partial class LoadingOverlayView : ReactiveUserControl<LoadingOverlayViewModel>
{
    public LoadingOverlayView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Message, v => v.MessageTextBlock.Text);
        });
    }

}
