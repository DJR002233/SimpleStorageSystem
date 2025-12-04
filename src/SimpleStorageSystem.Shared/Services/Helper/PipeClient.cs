using System.IO.Pipes;
using System.Text.Json;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Shared.Services.Helper;

public class PipeClient
{
    public static async Task<IpcResponse<resT>> GetIpcResponseAsynca<resT, reqT>(NamedPipeClientStream pipeClient, IpcRequest<reqT> ipcRequest)
    {

        using var writer = new StreamWriter(pipeClient, leaveOpen: true) { AutoFlush = true };
        using var reader = new StreamReader(pipeClient, leaveOpen: true);

        string json = JsonSerializer.Serialize(ipcRequest);
        await writer.WriteLineAsync(json);

        string? line = await reader.ReadLineAsync();
        if (String.IsNullOrWhiteSpace(line)) throw new Exception("No Response!");

        var ipcResponse = JsonSerializer.Deserialize<IpcResponse<resT>>(line);

        return ipcResponse!;
    }

    public static async Task<IpcResponse> GetIpcResponseAsync(NamedPipeClientStream pipeClienta, IpcRequest ipcRequest)
    {
        using var pipeClient = new NamedPipeClientStream("SimpleStorageAppAuthPipe");
        await pipeClient.ConnectAsync(200);
        using var writer = new StreamWriter(pipeClient) { AutoFlush = true };
        using var reader = new StreamReader(pipeClient);

        string json = JsonSerializer.Serialize(ipcRequest);
        await writer.WriteLineAsync(json);

        string? line = await reader.ReadLineAsync();
        if (String.IsNullOrWhiteSpace(line)) throw new Exception("No Response!");

        var ipcResponse = JsonSerializer.Deserialize<IpcResponse>(line);

        return ipcResponse!;
    }

}
