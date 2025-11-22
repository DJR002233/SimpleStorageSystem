using Microsoft.Extensions.DependencyInjection;
using System;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Auth;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class ViewModelCollection
{
    public static IServiceCollection InitializeViewModelServices(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<CreateAccountViewModel>();
        services.AddTransient<MainMenuViewModel>();

        services.AddSingleton<Func<LoginViewModel>>(sp => () => sp.GetRequiredService<LoginViewModel>());
        services.AddSingleton<Func<CreateAccountViewModel>>(sp => () => sp.GetRequiredService<CreateAccountViewModel>());
        services.AddSingleton<Func<MainMenuViewModel>>(sp => () => sp.GetRequiredService<MainMenuViewModel>());
        
        return services;
    }
}