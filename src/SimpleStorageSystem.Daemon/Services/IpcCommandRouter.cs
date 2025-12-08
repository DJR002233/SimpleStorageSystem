using SimpleStorageSystem.Daemon.Commands;
using SimpleStorageSystem.Shared.Enums;
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
        try
        {
            IpcCommand ipcCommand = request.Command ?? throw new Exception("Command is null");
            if (!_commandHandler.TryGetValue(ipcCommand, out var handler))
                return IpcResponse.CreateFromIpcRequest(request, IpcStatus.ERROR, $"Command not found. Command: {request.Command}");

            return await handler.HandleAsync(request);
        }
        catch (Exception ex)
        {
            return IpcResponse.CreateFromIpcRequest(request, IpcStatus.ERROR, ex.Message);
        }
    }

}
