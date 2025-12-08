using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Shared.Models;

public class IpcResponse
{
    public IpcType? Type { get; set; } = IpcType.Response;
    public required string RequestId { get; set; }
    public IpcStatus? Status { get; set; }
    public string? Payload { get; set; }

    public static IpcResponse Create(IpcStatus status, string? payload = null)
    {
        return new IpcResponse
        {
            RequestId = Guid.NewGuid().ToString(),
            Status = status,
            Payload = payload,
        };
    }

    public static IpcResponse CreateFromIpcRequest(IpcRequest request, IpcStatus status, string? payload = null)
    {
        return new IpcResponse
        {
            RequestId = request.RequestId,
            Status = status,
            Payload = payload,
        };
    }
}

public class IpcResponse<T> : IpcResponse
{
    public new T? Payload { get; set; }

    public static IpcResponse<T> Create(IpcStatus status, T payload)
    {
        return new IpcResponse<T>
        {
            RequestId = Guid.NewGuid().ToString(),
            Status = status,
            Payload = payload,
        };
    }

    public static IpcResponse<T> CreateFromIpcRequest(IpcRequest request, IpcStatus status, T payload)
    {
        return new IpcResponse<T>
        {
            RequestId = request.RequestId,
            Status = status,
            Payload = payload,
        };
    }
}
