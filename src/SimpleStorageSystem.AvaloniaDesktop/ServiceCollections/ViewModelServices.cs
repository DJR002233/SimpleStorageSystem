using Microsoft.Extensions.DependencyInjection;
using System;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class ViewModelCollection
{
    public static IServiceCollection InitializeViewModelServices(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MainMenuViewModel>();
        
        services.AddTransient<Func<MainMenuViewModel>>(sp => () => sp.GetRequiredService<MainMenuViewModel>());
        services.AddTransient<IOverlay>( sp => sp.GetRequiredService<MainWindowViewModel>());

        services.AddSingleton<INavigation, RouterHandler>();
        
        return services;
    }
}
