using Microsoft.Extensions.DependencyInjection;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Services.Main;

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

        services.AddTransient<AuthService>();
        services.AddTransient<AccountService>();

        return services;
    }
}