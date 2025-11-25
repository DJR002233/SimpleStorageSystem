using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using SimpleStorageSystem.AvaloniaDesktop.Services.CredentialsStore;
using SimpleStorageSystem.AvaloniaDesktop.Handler;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.AvaloniaDesktop.Handler.HttpHandler;
using SimpleStorageSystem.AvaloniaDesktop.AutoMapperProfiles;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class BaseCollection
{
    public static IServiceCollection InitializeBaseServices(this IServiceCollection services)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            services.AddSingleton<ICredentialsStore, LinuxSecretToolStore>();
        else
            throw new PlatformNotSupportedException();

        services.AddAutoMapper(typeof(ResponseProfile));

        services.AddSingleton<OnUnauthorizedHandler>();
        services.AddSingleton<LoadingOverlay>();
        services.AddSingleton<Session>();

        services.AddTransient<HttpSocketExceptionHandler>();
        services.AddTransient<AccessTokenHeaderHttpHandler>();
        services.AddTransient<RefreshTokenHeaderHttpHandler>();

        return services;
    }
}