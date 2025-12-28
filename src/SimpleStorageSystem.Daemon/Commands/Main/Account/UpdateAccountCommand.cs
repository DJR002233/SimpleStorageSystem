using System.Net;
using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.Main;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Commands.Main.Account;

public class UpdateAccountCommand : IIpcCommandHandler
{
    private readonly AccountService _accountService;
    public IpcCommand Command => IpcCommand.UpdateAccount;

    public UpdateAccountCommand(AccountService accountService)
    {
        _accountService = accountService;
    }

    public async ValueTask<IpcResponse> HandleAsync(IpcRequest request)
    {
        var payload = JsonSerializer.Deserialize<UpdateAccountRequest>((JsonElement)request.Payload!);
        ApiResponse apiResponse = await _accountService.UpdateAccountInformationAsync(payload!.Username, payload.Email, payload.Password);

        IpcStatus ipcStatus = IpcStatus.Failed;

        if (apiResponse.StatusCode is not null && (int)apiResponse.StatusCode < 300) ipcStatus = IpcStatus.Ok;
        else if (apiResponse.StatusCode == HttpStatusCode.InternalServerError) ipcStatus = IpcStatus.Error;

        return IpcResponse.CreateFromIpcRequest(request, ipcStatus, apiResponse.Message);
    }
}
