using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using ReactiveUI.Avalonia;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Components;

public partial class ConfirmationDialogBoxView : ReactiveUserControl<ConfirmationDialogBoxViewModel>
{
    public ConfirmationDialogBoxView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Title, v => v.TitleTextBlock.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Message, v => v.MessageTextBlock.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.OkButtonIsVisible, v => v.OkButton.IsVisible).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.ConfirmCancelButtonIsVisible, v => v.ConfirmButton.IsVisible).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.ConfirmCancelButtonIsVisible, v => v.CancelButton.IsVisible).DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.ConfirmCommand, v => v.OkButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.ConfirmCommand, v => v.ConfirmButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.CancelButton).DisposeWith(disposables);
        });
    }

}
