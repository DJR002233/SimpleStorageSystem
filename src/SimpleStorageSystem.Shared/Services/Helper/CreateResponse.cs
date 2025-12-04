using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Shared.Services.Helper;

public class CreateApiResponse
{
    public static ApiResponse<T> Success<T>(T? data, string? message = null)
    {
        return new ApiResponse<T>
        {
            StatusMessage = ApiStatus.Success,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failed<T>(string? message = null)
    {
        return new ApiResponse<T>
        {
            StatusMessage = ApiStatus.Failed,
            Message = message
        };
    }

    public static ApiResponse<T> Unauthenticated<T>(string? message = null)
    {
        return new ApiResponse<T>
        {
            Title = ApiStatus.Unauthenticated.ToString(),
            StatusMessage = ApiStatus.Unauthenticated,
            Message = message
        };
    }

    public static ApiResponse Success(string? message = null)
    {
        return Success<object>(new { }, message);
    }

    public static ApiResponse Failed(string? message = null)
    {
        return Failed<object>(message);
    }
    
    public static ApiResponse Unauthenticated(string? message = null, string? title = null)
    {
        return Unauthenticated<object>(message);
    }
    
}