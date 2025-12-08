using System.Runtime.InteropServices;
using SimpleStorageSystem.Daemon.Handler.HttpHandler;
using SimpleStorageSystem.Daemon.Services;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;
using SimpleStorageSystem.Daemon.Services.Auth.TokenStore;
using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.AutoMapperProfiles;

namespace SimpleStorageSystem.Daemon.ServiceCollections;

public static class BaseCollection
{
    public static IServiceCollection InitializeBaseServices(this IServiceCollection services)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            services.AddSingleton<ICredentialStore, LinuxSecretToolStore>();
        else
            throw new PlatformNotSupportedException();

        services.AddAutoMapper(typeof(ApiResponseProfile));

        services.AddTransient<AccessTokenHeaderHttpHandler>();
        services.AddTransient<RefreshTokenHeaderHttpHandler>();
        services.AddTransient<HttpSocketExceptionHandler>();

        services.AddSingleton<ITokenStore, TokenStore>();

        services.AddTransient<AuthService>();
        services.AddTransient<AccountService>();

        services.AddSingleton<PipeServer>();

        return services;
    }
}