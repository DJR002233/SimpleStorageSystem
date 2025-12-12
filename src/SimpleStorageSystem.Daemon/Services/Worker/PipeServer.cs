using System.IO.Pipes;
using System.Text.Json;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Services.Worker;

public class PipeServer
{
    private readonly IpcCommandRouter _commandRouter;

    public PipeServer(IpcCommandRouter commandRouter)
    {
        _commandRouter = commandRouter;
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var pipeServer = new NamedPipeServerStream(
                "SimpleStorageDaemon",
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous
            );

            await pipeServer.WaitForConnectionAsync(stoppingToken);

            using var reader = new StreamReader(pipeServer, leaveOpen: true);
            using var writer = new StreamWriter(pipeServer, leaveOpen: true) { AutoFlush = true };

            string? request = await reader.ReadLineAsync(stoppingToken);
            if (request == null) continue;

            var ipcRequest = JsonSerializer.Deserialize<IpcRequest>(request);
            if (ipcRequest is null) continue;

            try
            {
                IpcResponse ipcResponse = await _commandRouter.DispatchAsync(ipcRequest);

                string response = JsonSerializer.Serialize(ipcResponse);
                await writer.WriteLineAsync(response);
            }
            catch (Exception ex)
            {
                IpcResponse ipcResponse = IpcResponse.CreateFromIpcRequest(ipcRequest, IpcStatus.ERROR, ex.Message);

                string json = JsonSerializer.Serialize(ipcResponse);

                await writer.WriteLineAsync(json);
            }

        }
    }
}
