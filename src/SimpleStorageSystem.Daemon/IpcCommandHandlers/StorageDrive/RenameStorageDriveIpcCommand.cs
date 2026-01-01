using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Exceptions;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.IpcCommands.StorageDrive;

public class RenameStorageDriveIpcCommand : IIpcCommandHandler
{
    private readonly StorageDriveService _storageDriveService;
    public IpcCommand Command => IpcCommand.RenameStorageDrive;

    public RenameStorageDriveIpcCommand(StorageDriveService storageDriveService)
    {
        _storageDriveService = storageDriveService;
    }

    public async ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        var payload = JsonSerializer.Deserialize<StorageDriveIpcDTO>((JsonElement)request.Payload!) ?? throw new ExpectedException("Payload is null");
        if (String.IsNullOrWhiteSpace(payload.Name)) throw new ExpectedException("Name is null");
        
        await _storageDriveService.RenameStorageDriveAsync(
            payload.StorageDriveId ?? throw new ExpectedException("Id is null"), 
            payload.Name
        );

        return IpcResponse.CreateFromIpcRequest(request, IpcStatus.Ok);
    }
}
