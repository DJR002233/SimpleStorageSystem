using SimpleStorageSystem.WebAPI.Models;

namespace SimpleStorageSystem.WebAPI.Services.Helper;

public class CreateResponse
{
    public static Response Success(string? message = null)
    {
        return new Response
        {
            StatusMessage = StatusMessage.Success,
            Message = message
        };
    }
    public static Response<T> Success<T>(T? data, string? message = null)
    {
        return new Response<T>
        {
            StatusMessage = StatusMessage.Success,
            Message = message,
            Data = data
        };
    }
    public static Response Failed(string? message = null)
    {
        return new Response
        {
            StatusMessage = StatusMessage.Failed,
            Message = message
        };
    }
    public static Response<T> Failed<T>(string? message = null)
    {
        return new Response<T>
        {
            StatusMessage = StatusMessage.Failed,
            Message = message
        };
    }
    public static Response Error(string? message = null)
    {
        return new Response
        {
            StatusMessage = StatusMessage.Error,
            Message = message
        };
    }
    public static Response<T> Error<T>(string? message = null)
    {
        return new Response<T>
        {
            StatusMessage = StatusMessage.Error,
            Message = message
        };
    }
}