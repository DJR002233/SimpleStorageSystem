using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands.Auth;

public class CreateAccountCommand : IIpcCommandHandler
{
    private readonly AuthService _authService;
    public IpcCommand Command => IpcCommand.CreateAccount;

    public CreateAccountCommand(AuthService authService)
    {
        _authService = authService;
    }

    public async Task<IpcResponse> HandleAsync(IpcRequest request)
    {
        var payload = JsonSerializer.Deserialize<CreateAccountRequest>((JsonElement)request.Payload!);
        ApiResponse apiResponse = await _authService.CreateAccountAsync(payload!.Username, payload.Email, payload.Password);

        bool isSuccess = apiResponse.StatusMessage == ApiStatus.Success;
        return IpcResponse.CreateFromIpcRequest(request, isSuccess ? IpcStatus.Ok : IpcStatus.Failed, apiResponse.Message);
    }
}
