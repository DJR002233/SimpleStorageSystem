using Microsoft.Extensions.DependencyInjection;
using SimpleStorageSystem.AvaloniaDesktop.Client.Main;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class BaseCollection
{
    public static IServiceCollection InitializeBaseServices(this IServiceCollection services)
    {
        services.AddSingleton<LoadingOverlay>();
        services.AddTransient<DialogBox>();

        #region  Clients
        services.AddTransient<StorageDriveClient>();
        #endregion Clients

        return services;
    }
}