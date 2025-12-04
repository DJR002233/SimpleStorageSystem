using System.IO.Pipes;
using System.Text.Json;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Helper;

namespace SimpleStorageSystem.Daemon.Services.PipeServers;

public class AuthPipeServer
{
    private readonly AuthService _authService;

    public AuthPipeServer(AuthService authService)
    {
        _authService = authService;
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var pipeServer = new NamedPipeServerStream(
                "SimpleStorageAppAuthPipe",
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous
            );

            await pipeServer.WaitForConnectionAsync(stoppingToken);

            using var reader = new StreamReader(pipeServer, leaveOpen: true);
            using var writer = new StreamWriter(pipeServer, leaveOpen: true) { AutoFlush = true };

            string? line = await reader.ReadLineAsync(stoppingToken);
            if (line == null) continue;

            var ipcRequest = JsonSerializer.Deserialize<IpcRequest>(line);
            if (ipcRequest is null) continue;

            try
            {
                if (ipcRequest.Command == IpcCommand.HAS_SESSION)
                {
                    bool hasSession = _authService.HasSession();
                    await writer.WriteLineAsync(JsonSerializer.Serialize(
                        ipcRequest.CreateIpcResponse(hasSession ? IpcStatus.OK : IpcStatus.FAILED))
                    );
                }
                else if (ipcRequest.Command == IpcCommand.LOGIN)
                {
                    var payload = JsonSerializer.Deserialize<LoginRequest>(ipcRequest.Payload);
                    ApiResponse apiResponse = await _authService.LoginAsync(payload!.Email, payload.Password);

                    bool isSuccess = apiResponse.StatusMessage == ApiStatus.Success;

                    IpcResponse ipcResponse = ipcRequest.CreateIpcResponse(isSuccess ? IpcStatus.OK : IpcStatus.FAILED, apiResponse.Message);

                    await writer.WriteLineAsync(JsonSerializer.Serialize(ipcResponse));
                }
                else if (ipcRequest.Command == IpcCommand.CREATE_ACCOUNT)
                {
                    var payload = JsonSerializer.Deserialize<CreateAccountRequest>(ipcRequest.Payload);
                    ApiResponse apiResponse = await _authService.CreateAccountAsync(payload!.Username, payload.Email, payload.Password);

                    bool isSuccess = apiResponse.StatusMessage == ApiStatus.Success;
                    IpcResponse ipcResponse = ipcRequest.CreateIpcResponse(isSuccess ? IpcStatus.OK : IpcStatus.FAILED, apiResponse.Message);

                    await writer.WriteLineAsync(JsonSerializer.Serialize(ipcResponse));
                }
                else if (ipcRequest.Command == IpcCommand.LOGOUT)
                {
                    ApiResponse apiResponse = await _authService.LogoutAsync();

                    bool isSuccess = apiResponse.StatusMessage == ApiStatus.Success;
                    IpcResponse ipcResponse = ipcRequest.CreateIpcResponse(isSuccess ? IpcStatus.OK : IpcStatus.FAILED, apiResponse.Message);

                    await writer.WriteLineAsync(JsonSerializer.Serialize(ipcResponse));
                }
            }
            catch (HttpRequestException ex)
            {
                IpcResponse ipcResponse = ipcRequest.CreateIpcResponse(IpcStatus.ERROR, ex.Message);
                await writer.WriteLineAsync(JsonSerializer.Serialize(ipcResponse));
            }
            catch (Exception ex)
            {
                IpcResponse ipcResponse = ipcRequest.CreateIpcResponse(IpcStatus.ERROR, ex.Message);
                await writer.WriteLineAsync(JsonSerializer.Serialize(ipcResponse));
            }

        }
    }
}
