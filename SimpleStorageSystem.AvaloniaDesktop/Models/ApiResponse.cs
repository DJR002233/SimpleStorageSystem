using System.Collections.Generic;

namespace SimpleStorageSystem.AvaloniaDesktop.Models;

public class ApiResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public StatusMessage? StatusMessage { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public string? TraceId { get; set; }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}
