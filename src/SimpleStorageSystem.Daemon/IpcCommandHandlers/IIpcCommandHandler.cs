using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.IpcCommands;

public interface IIpcCommandHandler
{
    IpcCommand Command { get; }
    ValueTask<IpcResponse> HandleAsync(IpcRequest request);
}
