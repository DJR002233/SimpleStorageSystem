
using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Auth;

public partial class CreateAccountView : ReactiveUserControl<CreateAccountViewModel>
{
    public CreateAccountView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.CreateAccountCommand, v => v.CreateAccountButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.BackCommand, v => v.BackButton).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.Username, v => v.UsernameTextBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Email, v => v.EmailTextBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Password, v => v.PasswordTextBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.RePassword, v => v.RePasswordTextBox.Text).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.LoadingOverlay.IsVisible, v => v.LoadingOverlay.Visible).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.LoadingOverlay.Message, v => v.LoadingOverlay.Message).DisposeWith(disposables);
        });
    }
}