using System.Net;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands.Auth;

public class LogoutCommand : IIpcCommandHandler
{
    private readonly AuthService _authService;
    public IpcCommand Command => IpcCommand.Logout;

    public LogoutCommand(AuthService authService)
    {
        _authService = authService;
    }

    public async ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        ApiResponse apiResponse = await _authService.LogoutAsync();

        bool isSuccess = apiResponse.StatusCode == HttpStatusCode.NoContent;

        return IpcResponse.CreateFromIpcRequest(request, isSuccess ? IpcStatus.Ok : IpcStatus.Failed, apiResponse.Message);
    }

}
