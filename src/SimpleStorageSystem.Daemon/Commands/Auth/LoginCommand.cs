using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands.Auth;

public class LoginCommand : IIpcCommandHandler
{
    private readonly AuthService _authService;
    public IpcCommand Command => IpcCommand.LOGIN;

    public LoginCommand(AuthService authService)
    {
        _authService = authService;
    }

    public async Task<IpcResponse> HandleAsync(IpcRequest request)
    {
        var payload = JsonSerializer.Deserialize<LoginRequest>((JsonElement)request.Payload!);
        ApiResponse apiResponse = await _authService.LoginAsync(payload!.Email, payload.Password);

        bool isSuccess = apiResponse.StatusMessage == ApiStatus.Success;

        return IpcResponse.CreateFromIpcRequest(request, isSuccess ? IpcStatus.OK : IpcStatus.FAILED, apiResponse.Message);
    }
}
