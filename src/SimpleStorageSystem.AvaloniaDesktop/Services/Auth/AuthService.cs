using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Helper;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public class AuthService
{

    public AuthService() { }

    public async Task<IpcResponse> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginRequest { Email = email, Password = password };
        var ipcRequest = IpcMessage.CreateIpcRequest(IpcCommand.LOGIN, loginRequest);

        try
        {
            using var client = new NamedPipeClientStream("SimpleStorageAppAuthPipe");
            await client.ConnectAsync(10000);

            using var writer = new StreamWriter(client, leaveOpen: true) { AutoFlush = true };
            using var reader = new StreamReader(client, leaveOpen: true);

            string json = JsonSerializer.Serialize(ipcRequest);
            await writer.WriteLineAsync(json);

            string? line = await reader.ReadLineAsync();
            if (String.IsNullOrWhiteSpace(line)) return ipcRequest.CreateIpcResponse(IpcStatus.ERROR, "No Response!");

            var ipcResponse = JsonSerializer.Deserialize<IpcResponse>(line);

            return ipcResponse!;
        }
        catch (Exception ex)
        {
            return ipcRequest.CreateIpcResponse(IpcStatus.ERROR, ex.Message);
        }
    }

    public async Task<IpcResponse> CreateAccountAsync(string username, string email, string password)
    {
        var createAccountRequest = new CreateAccountRequest { Username = username, Email = email, Password = password };
        var ipcRequest = IpcMessage.CreateIpcRequest(IpcCommand.LOGIN, createAccountRequest);

        try
        {
            using var pipeClient = new NamedPipeClientStream("SimpleStorageAppAuthPipe");
            await pipeClient.ConnectAsync(200);
            return await PipeClient.GetIpcResponseAsync(pipeClient, ipcRequest);
        }
        catch (Exception ex)
        {
            return ipcRequest.CreateIpcResponse(IpcStatus.ERROR, ex.Message);
        }

    }

    public async Task<IpcResponse> LogoutAsync()
    {
        var ipcRequest = IpcMessage.CreateIpcRequest(IpcCommand.LOGOUT);

        try
        {
            using var pipeClient = new NamedPipeClientStream("SimpleStorageAppAuthPipe");
            await pipeClient.ConnectAsync(200);
            return await PipeClient.GetIpcResponseAsync(pipeClient, ipcRequest);
        }
        catch (Exception ex)
        {
            return ipcRequest.CreateIpcResponse(IpcStatus.ERROR, ex.Message);
        }

    }

    public async Task<IpcResponse> HasSessionAsync()
    {
        var ipcRequest = IpcMessage.CreateIpcRequest(IpcCommand.HAS_SESSION);

        try
        {
            using var pipeClient = new NamedPipeClientStream("SimpleStorageAppAuthPipe");
            await pipeClient.ConnectAsync(200);
            return await PipeClient.GetIpcResponseAsync(pipeClient, ipcRequest);
        }
        catch (Exception ex)
        {
            return ipcRequest.CreateIpcResponse(IpcStatus.ERROR, ex.Message);
        }

    }

}

/*
var loginRequest = new LoginRequest { Email = email, Password = password };
        var ipcRequest = IpcMessage.CreateIpcRequest(IpcCommand.LOGIN, loginRequest);

        try
        {
            using var client = new NamedPipeClientStream("SimpleStorageAppAuthPipe");
            await client.ConnectAsync(200);

            using var writer = new StreamWriter(client) { AutoFlush = true };
            using var reader = new StreamReader(client);

            string json = JsonSerializer.Serialize(ipcRequest);
            await writer.WriteLineAsync(json);

            string? line = await reader.ReadLineAsync();
            if (String.IsNullOrWhiteSpace(line)) return ipcRequest.CreateIpcResponse(IpcStatus.ERROR, "No Response!");

            var ipcResponse = JsonSerializer.Deserialize<IpcResponse>(line);

            return ipcResponse!;
        }
        catch (Exception ex)
        {
            return ipcRequest.CreateIpcResponse(IpcStatus.ERROR, ex.Message);
        }
*/
