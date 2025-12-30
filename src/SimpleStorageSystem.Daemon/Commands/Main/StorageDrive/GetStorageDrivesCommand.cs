using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands.Main.StorageDrive;

public class GetStorageDrivesCommand : IIpcCommandHandler
{
    private readonly StorageDriveService _storageDriveService;
    public IpcCommand Command => IpcCommand.GetStorageDriveList;

    public GetStorageDrivesCommand(StorageDriveService storageDriveService)
    {
        _storageDriveService = storageDriveService;
    }

    public async ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        List<StorageDriveIpcDTO> data = await _storageDriveService.GetStorageDrives();

        return IpcResponse.CreateFromIpcRequest(request, IpcStatus.Ok, data);
    }
}
