using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Services.Worker;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;
using SimpleStorageSystem.Daemon.Services.Main;

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

        // Services (service classes)
        services.AddTransient<StorageDriveService>();

        // Workers (background services)
        services.AddSingleton<PipeServer>();
        services.AddSingleton<StorageDriveSyncer>();

        return services;
    }
}