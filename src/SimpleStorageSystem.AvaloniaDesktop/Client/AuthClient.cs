using System;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.AvaloniaDesktop.Client;

public class AuthClient
{
    public async Task<IpcResponse> RequestLoginAsync(string email, string password)
    {
        var loginRequest = new LoginRequest { Email = email, Password = password };
        var ipcRequest = IpcRequest.Create(IpcCommand.Login, loginRequest);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse();
        }
        catch (Exception ex)
        {
            return IpcResponse.Create(IpcStatus.Error, ex.Message);
        }
    }

    public async Task<IpcResponse> RequestCreateAccountAsync(string username, string email, string password)
    {
        var createAccountRequest = new CreateAccountRequest { Username = username, Email = email, Password = password };
        var ipcRequest = IpcRequest.Create(IpcCommand.CreateAccount, createAccountRequest);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse();
        }
        catch (Exception ex)
        {
            return IpcResponse.Create(IpcStatus.Error, ex.Message);
        }

    }

    public async Task<IpcResponse> RequestLogoutAsync()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.Logout);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse();
        }
        catch (Exception ex)
        {
            return IpcResponse.Create(IpcStatus.Error, ex.Message);
        }

    }

    public async Task<IpcResponse> RequestHasSessionAsync()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.HasSession);

        try
        {
            using var pipeClient = new PipeClient();
            
            await pipeClient.PostMessageAsync(ipcRequest);

            return pipeClient.GetResponse();
        }
        catch (Exception ex)
        {
            return IpcResponse.Create(IpcStatus.Error, ex.Message);
        }

    }

}
