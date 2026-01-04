using Microsoft.Extensions.DependencyInjection;
using System;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;
using SimpleStorageSystem.AvaloniaDesktop.Handler;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class ViewModelCollection
{
    public static IServiceCollection InitializeViewModelServices(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MainMenuViewModel>();

        services.AddSingleton<INavigation, RouterHandler>();
        
        services.AddSingleton<Func<MainMenuViewModel>>(sp => () => sp.GetRequiredService<MainMenuViewModel>());
        
        return services;
    }
}
