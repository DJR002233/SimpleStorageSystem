
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Auth;

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.LoginCommand, v => v.LoginButton).DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.CreateAccountViewCommand, v => v.CreateAccountButton).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.Email, v => v.EmailTextBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Password, v => v.PasswordTextBox.Text).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.LoadingOverlay.IsVisible, v => v.LoadingOverlay.Visible).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.LoadingOverlay.Message, v => v.LoadingOverlay.Message).DisposeWith(disposables);
        });

        /*
        this.AttachedToVisualTree += async (_, __) =>
        {
            IsEnabled = false;
            try
            {
                if (DataContext is LoginViewModel vm)
                    await vm.InitializeAsync();
            }
            finally
            {
                IsEnabled = true;
            }
        };/**/
    }
}