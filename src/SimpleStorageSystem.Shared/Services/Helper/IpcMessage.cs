using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Shared.Services.Helper;

public static class IpcMessage
{
    public static IpcRequest<T> CreateIpcRequest<T>(IpcCommand command, T payload)
    {
        return new IpcRequest<T>
        {
            Type = IpcType.Request,
            Command = command,
            RequestId = Guid.NewGuid().ToString(),
            Payload = payload,
        };
    }

    public static IpcResponse<T> CreateIpcResponse<T>(this IpcRequest request, IpcStatus status, T payload)
    {
        return new IpcResponse<T>
        {
            Type = IpcType.Response,
            RequestId = request.RequestId,
            Status = status,
            Payload = payload,
        };
    }

    public static IpcRequest CreateIpcRequest(IpcCommand command)
    {
        return CreateIpcRequest<object>(command, new { });
    }

    public static IpcResponse CreateIpcResponse(this IpcRequest request, IpcStatus status, string? payload = null)
    {
        return new IpcResponse
        {
            Type = IpcType.Response,
            RequestId = request.RequestId,
            Status = status,
            Payload = payload,
        };
    }

}
