using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Shared.Models;

public class IpcResponse
{
    public IpcType Type { get; set; }
    public string? RequestId { get; set; }
    public IpcStatus Status { get; set; }
    public string? Payload { get; set; }
}

public class IpcResponse<T> : IpcResponse
{
    public new T? Payload { get; set; }
}
