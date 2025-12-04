using System.Text.Json;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Shared.Requests;

public class IpcRequest
{
    public IpcType Type { get; set; }
    public IpcCommand Command { get; set; }
    public string? RequestId { get; set; } = Guid.NewGuid().ToString();
    public JsonElement Payload { get; set; }
}

public class IpcRequest<T> : IpcRequest
{
    public new T? Payload { get; set; }
}
