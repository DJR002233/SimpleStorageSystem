using System.Net;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Shared.Models;

public class ApiResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public HttpStatusCode? StatusCode { get; set; }
    public ApiStatus? StatusMessage { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public string? TraceId { get; set; }
    public object? Data { get; set; }
}

public class ApiResponse<T> : ApiResponse
{
    // public T? Data { get; set; }
    public new T? Data { get; set; }
}
