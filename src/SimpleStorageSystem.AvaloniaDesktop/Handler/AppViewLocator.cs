using System;
using System.Reflection;
using Avalonia.Controls;
using ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler;

public class AppViewLocator : IViewLocator
{
    // private readonly INavigation Navigation;
    private string? currentView = null;
    public IViewFor? ResolveView<T>(T viewModel, string? contract = null)
    {
        var viewModelType = viewModel?.GetType();
        var viewTypeName = viewModelType?.FullName?.Replace("ViewModel", "View");

        if (viewTypeName == currentView)
            return null;

        if (viewTypeName == null)
            throw new ArgumentNullException(nameof(viewModel));

        var viewType = Assembly.GetExecutingAssembly().GetType(viewTypeName);

        if (viewType == null)
            throw new ArgumentOutOfRangeException(nameof(viewModel), $"No view found for {viewModelType?.Name}");

        var view = (Control)Activator.CreateInstance(viewType)!;
        view.DataContext = viewModel;
        currentView = viewTypeName;
        // Navigation.Transition = null;
        return (IViewFor)view;/**/
        /*
        return viewModel switch
        {
            LoginViewModel context => new LoginView { DataContext = context },
            //LoginViewModel context => new LoginView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };/**/
    }

    // public AppViewLocator()
    // {
    //     Navigation = App.Services.GetRequiredService<INavigation>();
    // }

}
