using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Main.Pages;

public partial class AccountPageView : ReactiveUserControl<AccountPageViewModel>
{
    public AccountPageView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.SaveButton).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.Username, v => v.UsernameTextBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Email, v => v.EmailTextBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Password, v => v.PasswordTextBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.RePassword, v => v.RePasswordTextBox.Text).DisposeWith(disposables);
        });

    }

}
