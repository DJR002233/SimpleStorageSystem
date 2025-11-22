namespace SimpleStorageSystem.WebAPI.Models;

public class Response
{
    public int? Status { get; set; }
    public StatusMessage? StatusMessage { get; set; }
    public string? Message { get; set; }
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