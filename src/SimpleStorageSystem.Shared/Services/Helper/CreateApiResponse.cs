using System.Net;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Shared.Services.Helper;

public class CreateApiResponse
{
    public static ApiResponse<T> Success<T>(T? data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Title = ApiStatus.Success.ToString(),
            // StatusMessage = ApiStatus.Success,
            StatusCode = data is not null || !String.IsNullOrWhiteSpace(message) ? HttpStatusCode.OK : HttpStatusCode.NoContent,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failed<T>(string? message = null, string? title = null)
    {
        return new ApiResponse<T>
        {
            Title = title ?? ApiStatus.Failed.ToString(),
            // StatusMessage = ApiStatus.Failed,
            Message = message
        };
    }

    public static ApiResponse<T> Unauthenticated<T>(string? message = null, string? title = null)
    {
        return new ApiResponse<T>
        {
            Title = title ?? ApiStatus.Unauthenticated.ToString(),
            // StatusMessage = ApiStatus.Unauthenticated,
            Message = message
        };
    }

    public static ApiResponse<T> Unauthorized<T>(string? message = null, string? title = null)
    {
        return new ApiResponse<T>
        {
            Title = title ?? ApiStatus.Unauthorized.ToString(),
            // StatusMessage = ApiStatus.Unauthorized,
            Message = message
        };
    }

    public static ApiResponse<T> Error<T>(string? message = null, string? title = null)
    {
        return new ApiResponse<T>
        {
            Title = title ?? ApiStatus.Error.ToString(),
            // StatusMessage = ApiStatus.Error,
            Message = message
        };
    }
    
    public static ApiResponse<T> ErrorFromException<T>(Exception exception, string? message = null)
    {
        return new ApiResponse<T>
        {
            Title = exception?.GetType().ToString(),
            // StatusMessage = ApiStatus.Error,
            Message = message ?? exception?.Message
        };
    }


    public static ApiResponse Success(string? message = null)
    {
        return Success<object>(new { }, message);
    }

    public static ApiResponse Failed(string? message = null, string? title = null)
    {
        return Failed<object>(message, title);
    }
    
    public static ApiResponse Unauthenticated(string? message = null, string? title = null)
    {
        return Unauthenticated<object>(message, title);
    }

    public static ApiResponse Unauthorized(string? message = null, string? title = null)
    {
        return Unauthorized<object>(message, title);
    }

    public static ApiResponse Error(string? message = null, string? title = null)
    {
        return Error<object>(message, title);
    }
    
    public static ApiResponse ErrorFromException(Exception exception, string? message = null)
    {
        return ErrorFromException<object>(exception, message);
    }
}