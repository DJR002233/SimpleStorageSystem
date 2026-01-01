using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Exceptions;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.IpcCommands.StorageDrive;

public class DisconnectStorageDriveIpcCommand : IIpcCommandHandler
{
    private readonly StorageDriveService _storageDriveService;
    public IpcCommand Command => IpcCommand.DisconnectStorageDrive;

    public DisconnectStorageDriveIpcCommand(StorageDriveService storageDriveService)
    {
        _storageDriveService = storageDriveService;
    }

    public async ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        var payload = JsonSerializer.Deserialize<StorageDriveIpcDTO>((JsonElement)request.Payload!) ?? throw new ExpectedException("Payload is null");
        if (payload.StorageDriveId is null) throw new ExpectedException("StorageDriveId is null");

        await _storageDriveService.DisconnectStorageDriveAsync(payload.StorageDriveId ?? throw new ExpectedException("Storage drive id is null"));

        return IpcResponse.CreateFromIpcRequest(request, IpcStatus.Ok);
    }
}
