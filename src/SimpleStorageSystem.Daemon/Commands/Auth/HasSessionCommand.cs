using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands.Auth;

public class HasSessionCommand : IIpcCommandHandler
{
    private readonly AuthService _authService;
    public IpcCommand Command => IpcCommand.HasSession;

    public HasSessionCommand(AuthService authService)
    {
        _authService = authService;
    }

    public ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        var hasSession = _authService.HasSession();

        return ValueTask.FromResult(IpcResponse.CreateFromIpcRequest(request, hasSession ? IpcStatus.Ok : IpcStatus.Failed));
    }
}
