using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.AvaloniaDesktop.Services;

public class PipeClient : IDisposable
{
    private readonly NamedPipeClientStream _pipeClient;
    private StreamWriter? _writer;
    private StreamReader? _reader;

    private int _timeout;
    private string? _response;

    public int Timeout
    {
        set => _timeout = value;
    }

    public PipeClient(int timeout = 5000, NamedPipeClientStream? pipeClient = null)
    {
        if (pipeClient is not null) _pipeClient = pipeClient;
        else _pipeClient = new NamedPipeClientStream("SimpleStorageDaemon");
        _timeout = timeout;
    }

    public async Task SendMessageAsync<T>(T request)
    {
        if (request is not IpcRequest)
            throw new Exception($"Invalid IpcRequest!");

        await _pipeClient.ConnectAsync(_timeout);

        _writer = new StreamWriter(_pipeClient, leaveOpen: true) { AutoFlush = true };

        var json = JsonSerializer.Serialize(request);
        
        await _writer.WriteLineAsync(json);
    }

    public async Task PostMessageAsync<T>(T request)
    {
        await SendMessageAsync(request);

        _reader = new StreamReader(_pipeClient, leaveOpen: true);
        
        _response = await _reader.ReadLineAsync();
    }

    public IpcResponse GetResponse()
    {
        if (String.IsNullOrWhiteSpace(_response))
            throw new Exception("No Response!");

        var res = JsonSerializer.Deserialize<IpcResponse>(_response)!;

        return res;
    }

    public IpcResponse<T> GetResponse<T>()
    {
        if (String.IsNullOrWhiteSpace(_response))
            throw new Exception("No Response!");

        var res = JsonSerializer.Deserialize<IpcResponse<T>>(_response)!;

        return res;
    }

    public void Dispose()
    {
        _writer?.Dispose();
        _reader?.Dispose();
        _pipeClient?.Dispose();
    }

}
