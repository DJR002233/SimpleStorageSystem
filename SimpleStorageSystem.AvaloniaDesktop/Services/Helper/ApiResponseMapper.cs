using System;
using SimpleStorageSystem.AvaloniaDesktop.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.Services;

public static class ApiResponseMapper
{
    public static Response<T> MapToResponse<T>(this ApiResponse<T> res)
    {
        if (res is null)
            throw new ArgumentNullException(nameof(res), "API validation error response cannot be null");

        string message = "";
        if (!String.IsNullOrWhiteSpace(res.Message))
            message = res.Message + "\n\n";

        if (res.Errors is not null)
            foreach (var key in res.Errors)
            {
                string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
                message += $"{combined}\n";
            }

        return new Response<T>
        {
            Title = res.Title,
            Status = res.Status,
            StatusMessage = res.StatusMessage,
            Message = message.Trim(),
            Data = res.Data
        };
    }

    public static Response MapToResponse(this ApiResponse res)
    {
        if (res is null)
            throw new ArgumentNullException(nameof(res), "API validation error response cannot be null");

        string message = "";
        if (!String.IsNullOrWhiteSpace(res.Message))
            message = res.Message + "\n\n";

        if (res.Errors is not null)
            foreach (var key in res.Errors)
            {
                string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
                message += $"{combined}\n";
            }

        return new Response
        {
            Title = res.Title,
            Status = res.Status,
            StatusMessage = res.StatusMessage,
            Message = message.Trim()
        };
    }

}/**/
