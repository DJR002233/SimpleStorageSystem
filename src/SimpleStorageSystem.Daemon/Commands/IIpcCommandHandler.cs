using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands;

public interface IIpcCommandHandler
{
    IpcCommand Command { get; }
    Task<IpcResponse> HandleAsync(IpcRequest request);
}
