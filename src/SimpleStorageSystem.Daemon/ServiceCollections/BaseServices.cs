using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Handler.HttpHandler;
using SimpleStorageSystem.Daemon.Services.Worker;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;
using SimpleStorageSystem.Daemon.Services.Auth.TokenStore;
using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Daemon.Services;

namespace SimpleStorageSystem.Daemon.ServiceCollections;

public static class BaseCollection
{
    public static IServiceCollection InitializeBaseServices(this IServiceCollection services)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            services.AddSingleton<ICredentialStore, LinuxSecretToolStore>();
        else
            throw new PlatformNotSupportedException();

        services.AddDbContext<SqLiteDbContext>(options =>
        {
            options.UseSqlite("Data Source=FileManagementRecord.db");
        });

        services.AddSingleton<SettingsManager>();

        services.AddTransient<HttpClientBaseConfigHandler>();
        services.AddTransient<HttpSocketExceptionHandler>();
        services.AddTransient<AccessTokenHeaderHandler>();
        services.AddTransient<RefreshTokenHeaderHandler>();

        services.AddSingleton<ITokenStore, TokenStore>();

        services.AddTransient<AuthService>();
        services.AddTransient<AccountService>();

        services.AddSingleton<PipeServer>();

        return services;
    }
}