using Microsoft.Extensions.DependencyInjection;
using SimpleStorageSystem.AvaloniaDesktop.Client;
using SimpleStorageSystem.AvaloniaDesktop.Client.Main;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class BaseCollection
{
    public static IServiceCollection InitializeBaseServices(this IServiceCollection services)
    {
        // if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        //     services.AddSingleton<ICredentialStore, LinuxSecretToolStore>();
        // else
        //     throw new PlatformNotSupportedException();

        services.AddSingleton<OnUnauthorizedHandler>();
        services.AddSingleton<LoadingOverlay>();

        services.AddTransient<AuthClient>();
        services.AddTransient<AccountClient>();
        services.AddTransient<StorageDriveClient>();

        return services;
    }
}