using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Results;

namespace SimpleStorageSystem.Daemon.Commands.Main.StorageDrive;

public class GetStorageDrivesCommand : IIpcCommandHandler
{
    private readonly StorageDriveService _storageDriveService;
    public IpcCommand Command => IpcCommand.UpdateAccount;

    public GetStorageDrivesCommand(StorageDriveService storageDriveService)
    {
        _storageDriveService = storageDriveService;
    }

    public async Task<IpcResponse> HandleAsync(IpcRequest request)
    {
        List<StorageDriveResult> data = await _storageDriveService.GetStorageDrives();

        return IpcResponse.CreateFromIpcRequest(request, IpcStatus.Ok, data);
    }
}
