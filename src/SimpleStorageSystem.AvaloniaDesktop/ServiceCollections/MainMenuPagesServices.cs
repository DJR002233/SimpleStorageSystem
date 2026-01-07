using Microsoft.Extensions.DependencyInjection;
using SimpleStorageSystem.AvaloniaDesktop.Services.PageFactory;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class MainMenuPagesCollection
{
    public static IServiceCollection InitializeMainMenuPagesServices(this IServiceCollection services)
    {
        services.AddTransient<IPageFactory<IMainMenuPage>, MainMenuPageFactory>();

        services.AddTransient<ActivityPageViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<StorageDrivesPageViewModel>();

        services.AddTransient<IMainMenuPage>(sp => sp.GetRequiredService<ActivityPageViewModel>());
        services.AddTransient<IMainMenuPage>(sp => sp.GetRequiredService<SettingsPageViewModel>());
        services.AddTransient<IMainMenuPage>(sp => sp.GetRequiredService<StorageDrivesPageViewModel>());

        return services;
    }
}
