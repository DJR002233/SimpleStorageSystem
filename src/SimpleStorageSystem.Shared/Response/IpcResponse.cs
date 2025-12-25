using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Shared.Models;

public class IpcResponse
{
    public IpcType? Type { get; set; } = IpcType.Response;
    public required string RequestId { get; set; }
    public IpcStatus? Status { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; init; }
    public object? Payload { get; set; }

    public static IpcResponse Create(IpcStatus status, string? message = null, object? payload = null)
    {
        return new IpcResponse
        {
            RequestId = Guid.NewGuid().ToString(),
            Status = status,
            Message = message,
            Payload = payload,
        };
    }

    public static IpcResponse CreateFromIpcRequest(IpcRequest request, IpcStatus status, string? message = null, object? payload = null)
    {
        return new IpcResponse
        {
            RequestId = request.RequestId,
            Status = status,
            Message = message,
            Payload = payload,
        };
    }

    public static IpcResponse CreateErrorResponse(string errorMessage)
    {
        return CreateErrorResponse<object>(errorMessage);
    }

    public static IpcResponse CreateErrorResponseFromIpcRequest(IpcRequest request, string errorMessage)
    {
        return CreateErrorResponseFromIpcRequest<object>(request, errorMessage);
    }


    public static IpcResponse<T> Create<T>(IpcStatus status, T? payload, string? message = null)
    {
        return new IpcResponse<T>
        {
            RequestId = Guid.NewGuid().ToString(),
            Status = status,
            Message = message,
            Payload = payload,
        };
    }

    public static IpcResponse<T> CreateFromIpcRequest<T>(IpcRequest request, IpcStatus status, T? payload, string? message = null)
    {
        return new IpcResponse<T>
        {
            RequestId = request.RequestId,
            Status = status,
            Message = message,
            Payload = payload,
        };
    }

    public static IpcResponse<T> Create<T>(IpcStatus status, string? message = null)
    {
        return new IpcResponse<T>
        {
            RequestId = Guid.NewGuid().ToString(),
            Status = status,
            Message = message,
        };
    }

    public static IpcResponse<T> CreateFromIpcRequest<T>(IpcRequest request, IpcStatus status, string? message = null)
    {
        return new IpcResponse<T>
        {
            RequestId = request.RequestId,
            Status = status,
            Message = message,
        };
    }
    
    public static IpcResponse<T> CreateErrorResponse<T>(string errorMessage)
    {
        return new IpcResponse<T>
        {
            RequestId = Guid.NewGuid().ToString(),
            Status = IpcStatus.Error,
            ErrorMessage = errorMessage,
        };
    }

    public static IpcResponse<T> CreateErrorResponseFromIpcRequest<T>(IpcRequest request, string errorMessage)
    {
        return new IpcResponse<T>
        {
            RequestId = request.RequestId,
            Status = IpcStatus.Error,
            ErrorMessage = errorMessage,
        };
    }

}

public class IpcResponse<T> : IpcResponse
{
    public new T? Payload { get; set; }
}

// public class IpcResponse
// {
//     public IpcType? Type { get; set; } = IpcType.Response;
//     public required string RequestId { get; set; }
//     public IpcStatus? Status { get; set; }
//     public object? Payload { get; set; }

//     public static IpcResponse Create(IpcStatus status, string? payload = null)
//     {
//         return new IpcResponse
//         {
//             RequestId = Guid.NewGuid().ToString(),
//             Status = status,
//             Payload = payload,
//         };
//     }

//     public static IpcResponse CreateFromIpcRequest(IpcRequest request, IpcStatus status, string? payload = null)
//     {
//         return new IpcResponse
//         {
//             RequestId = request.RequestId,
//             Status = status,
//             Payload = payload,
//         };
//     }
// }

// public class IpcResponse<T> : IpcResponse
// {
//     public new T? Payload { get; set; }

//     public static IpcResponse<T> Create(IpcStatus status, T payload)
//     {
//         return new IpcResponse<T>
//         {
//             RequestId = Guid.NewGuid().ToString(),
//             Status = status,
//             Payload = payload,
//         };
//     }

//     public static IpcResponse<T> CreateFromIpcRequest(IpcRequest request, IpcStatus status, T payload)
//     {
//         return new IpcResponse<T>
//         {
//             RequestId = request.RequestId,
//             Status = status,
//             Payload = payload,
//         };
//     }
// }
