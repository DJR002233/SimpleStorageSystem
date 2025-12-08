using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands.Main.Account;

public class UpdateAccountCommand : IIpcCommandHandler
{
    private readonly AccountService _accountService;
    public IpcCommand Command => IpcCommand.UPDATE_ACCOUNT;

    public UpdateAccountCommand(AccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<IpcResponse> HandleAsync(IpcRequest request)
    {
        var payload = JsonSerializer.Deserialize<UpdateAccountRequest>((JsonElement)request.Payload!);
        ApiResponse apiResponse = await _accountService.UpdateAccountInformationAsync(payload!.Username, payload.Email, payload.Password);

        bool isSuccess = apiResponse.StatusMessage == ApiStatus.Success;
        return IpcResponse.CreateFromIpcRequest(request, isSuccess ? IpcStatus.OK : IpcStatus.FAILED, apiResponse.Message);
    }
}
