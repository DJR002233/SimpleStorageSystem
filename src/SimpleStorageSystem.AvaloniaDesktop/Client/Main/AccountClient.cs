using System;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.AvaloniaDesktop.Client.Main;

public class AccountClient
{

    public async ValueTask<IpcResponse> RequestUpdateAccountInformation(string? username, string? email, string? password)
    {
        var data = new UpdateAccountRequest { Username = username, Email = email, Password = password };
        var ipcRequest = IpcRequest.Create(IpcCommand.UpdateAccount, data);
        
        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateFromIpcRequest(ipcRequest, IpcStatus.Error, ex.Message);
        }
    }
    
}
