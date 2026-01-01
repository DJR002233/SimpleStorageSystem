using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.StorageServerControllers;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Exceptions;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.IpcCommands.StorageDrive;

public class AddStorageDriveIpcCommand : IIpcCommandHandler
{
    private readonly IEnumerable<IStorageServerController> _storageServerControllers;
    public IpcCommand Command => IpcCommand.AddStorageDrive;

    public AddStorageDriveIpcCommand(IEnumerable<IStorageServerController> storageServerControllers)
    {
        _storageServerControllers = storageServerControllers;
    }

    public async ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        var payload = JsonSerializer.Deserialize<StorageDriveIpcDTO>((JsonElement)request.Payload!) ?? throw new ExpectedException("Payload is null");

        if (String.IsNullOrWhiteSpace(payload.Name)) throw new ExpectedException("Name is null");
        if(payload.StorageServer is null) throw new ExpectedException("Storage server is not defined");
        
        var controller = _storageServerControllers.SingleOrDefault( c => c.StorageServer == payload.StorageServer);
        if (controller is null) throw new Exception("Controller not found");
        
        await controller.LoginAsync(payload.Name);

        return IpcResponse.CreateFromIpcRequest(request, IpcStatus.Ok);
    }
}
