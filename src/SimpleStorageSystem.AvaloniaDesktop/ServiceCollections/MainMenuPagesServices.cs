using Microsoft.Extensions.DependencyInjection;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class MainMenuPagesCollection
{
    public static IServiceCollection InitializeMainMenuPagesServices(this IServiceCollection services)
    {
        services.AddTransient<AccountPageViewModel>();
        services.AddTransient<ActivityPageViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<StorageDevicesPageViewModel>();
        
        return services;
    }
}
