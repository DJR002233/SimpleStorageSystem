using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Services.StorageServerControllers;

public class SimpleStorageServerController : IStorageServerController
{
    public SupportedStorageServer StorageServer => SupportedStorageServer.SimpleStorageServer;

    public async ValueTask LoginAsync(string name)
    {
        await Task.Delay(1000);
        throw new Exception("unimplemented");
    }

}
