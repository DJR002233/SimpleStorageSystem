using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.IpcCommands.StorageDrive;

public class GetStorageDriveIpcCommand : IIpcCommandHandler
{
    private readonly StorageDriveService _storageDriveService;
    public IpcCommand Command => IpcCommand.GetStorageDriveList;

    public GetStorageDriveIpcCommand(StorageDriveService storageDriveService)
    {
        _storageDriveService = storageDriveService;
    }

    public async ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        List<StorageDriveIpcDTO> data = await _storageDriveService.GetStorageDrivesAsync();

        return IpcResponse.CreateFromIpcRequest(request, IpcStatus.Ok, data);
    }
}
