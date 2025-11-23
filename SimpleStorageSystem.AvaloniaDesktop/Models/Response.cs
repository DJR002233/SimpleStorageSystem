using System.Net;
using System.Collections.Generic;

namespace SimpleStorageSystem.AvaloniaDesktop.Models;

public class Response
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public HttpStatusCode? StatusCode { get; set; }
    public StatusMessage? StatusMessage { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public string? TraceId { get; set; }
    public object? Data { get; set; }
}

public class Response<T> : Response
{
    public new T? Data { get; set; }
}

public enum StatusMessage
{
    Success,
    Failed,
    Error,
    NotFound,
    Unauthenticated,
    Unauthorized,
    Timeout,
    ValidationError
}