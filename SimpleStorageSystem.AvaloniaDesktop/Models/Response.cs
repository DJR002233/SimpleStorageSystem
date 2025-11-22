using System;
using System.Net;
using System.Net.Http;

namespace SimpleStorageSystem.AvaloniaDesktop.Models;

public class Response
{
    public string? Title { get; set; } = "";
    public int? Status { get; set; }
    public HttpStatusCode? StatusCode { get; set; }
    public StatusMessage? StatusMessage { get; set; }
    public string? Message { get; set; } = "";
}

public class Response<T> : Response
{
    public T? Data { get; set; }
}

public enum StatusMessage
{
    Success,
    Failed,
    Error,
    NotFound,
    Unauthorized,
    Timeout,
    ValidationError
}
