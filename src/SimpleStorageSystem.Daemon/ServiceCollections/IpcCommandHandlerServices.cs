using SimpleStorageSystem.Daemon.Commands;
using SimpleStorageSystem.Daemon.Commands.Main.StorageDrive;
using SimpleStorageSystem.Daemon.Services;

namespace SimpleStorageSystem.Daemon.ServiceCollections;

public static class IpcCommandHandlerCollection
{
    public static IServiceCollection InitializeIpcCommandHandlerServices(this IServiceCollection services)
    {
        #region Auth CommandHandlers
        // services.AddTransient<IIpcCommandHandler, LoginCommand>();
        // services.AddTransient<IIpcCommandHandler, CreateAccountCommand>();
        // services.AddTransient<IIpcCommandHandler, HasSessionCommand>();
        // services.AddTransient<IIpcCommandHandler, LogoutCommand>();
        #endregion Auth CommandHandlers

        #region Account CommandHandlers
        // services.AddTransient<IIpcCommandHandler, UpdateAccountCommand>();
        #endregion Account CommandHandlers

        #region StorageDrive CommandHandlers
        services.AddTransient<IIpcCommandHandler, GetStorageDriveIpcCommand>();
        services.AddTransient<IIpcCommandHandler, AddStorageDriveIpcCommand>();
        services.AddTransient<IIpcCommandHandler, RenameStorageDriveIpcCommand>();
        services.AddTransient<IIpcCommandHandler, DisconnectStorageDriveIpcCommand>();
        #endregion StorageDrive CommandHandlers

        services.AddTransient<IpcCommandRouter>();
        
        return services;
    }
}