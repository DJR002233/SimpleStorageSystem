using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using ReactiveUI.Avalonia;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Components;

public partial class MessageDialogBoxView : ReactiveUserControl<MessageDialogBoxViewModel>
{
    public MessageDialogBoxView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Title, v => v.TitleTextBlock.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Message, v => v.MessageTextBlock.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Text, v => v.InputTextBox.Text).DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.ConfirmCommand, v => v.ConfirmButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.CancelButton).DisposeWith(disposables);
        });
    }

}
