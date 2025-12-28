using System;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.AvaloniaDesktop.Client;

public class AuthClient
{
    public async ValueTask<IpcResponse> RequestLoginAsync(string email, string password)
    {
        var data = new LoginRequest { Email = email, Password = password };
        var ipcRequest = IpcRequest.Create(IpcCommand.Login, data);

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

    public async ValueTask<IpcResponse> RequestCreateAccountAsync(string username, string email, string password)
    {
        var data = new CreateAccountRequest { Username = username, Email = email, Password = password };
        var ipcRequest = IpcRequest.Create(IpcCommand.CreateAccount, data);

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

    public async ValueTask<IpcResponse> RequestLogoutAsync()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.Logout);

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

    public async ValueTask<IpcResponse> RequestHasSessionAsync()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.HasSession);

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
