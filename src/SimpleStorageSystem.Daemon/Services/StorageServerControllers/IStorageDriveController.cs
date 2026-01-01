using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Services.StorageServerControllers;

public interface IStorageServerController
{
    SupportedStorageServer StorageServer { get; }
    ValueTask LoginAsync(string name);
}
