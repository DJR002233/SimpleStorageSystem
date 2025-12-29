using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Shared.Requests;

public class IpcRequest
{
    public IpcType? Type { get; set; } = IpcType.Request;
    public IpcCommand? Command { get; set; }
    public required string RequestId { get; set; }// = Guid.NewGuid().ToString();
    public object? Payload { get; set; }

    public static IpcRequest Create(IpcCommand ipcCommand, object? payload = null)
    {
        return new IpcRequest
        {
            Command = ipcCommand,
            RequestId = Guid.NewGuid().ToString(),
            Payload = payload,
        };
    }

    public static IpcRequest CreateFromIpcResponse(IpcResponse response, IpcCommand ipcCommand, object? payload = null)
    {
        return new IpcRequest
        {
            Command = ipcCommand,
            RequestId = response.RequestId,
            Payload = payload,
        };
    }


    public static IpcRequest<T> Create<T>(IpcCommand ipcCommand, T payload)
    {
        return new IpcRequest<T>
        {
            Command = ipcCommand,
            RequestId = Guid.NewGuid().ToString(),
            Payload = payload,
        };
    }

    public static IpcRequest<T> CreateFromIpcResponse<T>(IpcResponse response, IpcCommand ipcCommand, T payload)
    {
        return new IpcRequest<T>
        {
            Command = ipcCommand,
            RequestId = response.RequestId,
            Payload = payload,
        };
    }
    
    public static IpcRequest<T> Create<T>(IpcCommand ipcCommand)
    {
        return new IpcRequest<T>
        {
            Command = ipcCommand,
            RequestId = Guid.NewGuid().ToString(),
        };
    }

    public static IpcRequest<T> CreateFromIpcResponse<T>(IpcResponse response, IpcCommand ipcCommand)
    {
        return new IpcRequest<T>
        {
            Command = ipcCommand,
            RequestId = response.RequestId,
        };
    }

}

public class IpcRequest<T> : IpcRequest
{
    public new T? Payload { get; set; }

}
