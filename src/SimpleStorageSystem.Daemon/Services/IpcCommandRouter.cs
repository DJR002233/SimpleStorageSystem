using SimpleStorageSystem.Daemon.IpcCommands;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Exceptions;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Services;

public class IpcCommandRouter
{
    private readonly Dictionary<IpcCommand, IIpcCommandHandler> _commandHandler;

    public IpcCommandRouter(IEnumerable<IIpcCommandHandler> commandHandler)
    {
        _commandHandler = commandHandler.ToDictionary(c => c.Command, c => c);
    }

    public async Task<IpcResponse> DispatchAsync(IpcRequest request)
    {
        IpcCommand ipcCommand = request.Command ?? throw new ExpectedException("Command is null");
        
        if (!_commandHandler.TryGetValue(ipcCommand, out var handler))
            throw new ExpectedException($"Command not found. Command: {request.Command}");

        return await handler.HandleAsync(request);
    }

}
