using System;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;
using SimpleStorageSystem.AvaloniaDesktop.Views.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Main;

public class MainMenuPageSwapper : IViewLocator
{
    public IViewFor? ResolveView<T>(T viewModel, string? contract = null)
    {
        return viewModel switch
        {
            CreateAccountViewModel context => new CreateAccountView { DataContext = context },
            //LoginViewModel context => new LoginView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };/**/
    }

}
