using System;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.AvaloniaDesktop.Client;

public class AuthClient
{
    public async ValueTask<IpcResponse<string>> RequestLoginAsync(string email, string password)
    {
        var data = new LoginRequest { Email = email, Password = password };
        var ipcRequest = IpcRequest.Create(IpcCommand.Login, data);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse<string>();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateErrorResponseFromIpcRequest<string>(ipcRequest, ex.Message);
        }
    }

    public async ValueTask<IpcResponse<string>> RequestCreateAccountAsync(string username, string email, string password)
    {
        var data = new CreateAccountRequest { Username = username, Email = email, Password = password };
        var ipcRequest = IpcRequest.Create(IpcCommand.CreateAccount, data);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse<string>();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateErrorResponseFromIpcRequest<string>(ipcRequest, ex.Message);
        }

    }

    public async ValueTask<IpcResponse<string>> RequestLogoutAsync()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.Logout);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse<string>();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateErrorResponseFromIpcRequest<string>(ipcRequest, ex.Message);
        }

    }

    public async ValueTask<IpcResponse<string>> RequestHasSessionAsync()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.HasSession);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse<string>();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateErrorResponseFromIpcRequest<string>(ipcRequest, ex.Message);
        }

    }

}
