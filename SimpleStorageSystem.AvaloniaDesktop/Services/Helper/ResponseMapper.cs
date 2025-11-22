using System;
using System.Net.Http;
using SimpleStorageSystem.AvaloniaDesktop.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

public class ResponseMapper
{
    public static Response MapHttpRequestException(HttpRequestException ex)
    {
        string message = "";
        
        if(!String.IsNullOrWhiteSpace(ex.StatusCode.ToString()))
            message = $"Error: {ex.StatusCode}";
        if(!String.IsNullOrWhiteSpace(ex.Message))
            message += $"\n\n{ex.Message}";

        if (String.IsNullOrWhiteSpace(message))
        {
            message = "Unknown Error!";
        }

        return new Response
        {
            Title = "Http Exception",
            StatusCode = ex.StatusCode,
            StatusMessage = StatusMessage.Error,
            Message = message
        };
    }
    public static Response MapException(Exception ex)
    {
        return new Response
        {
            Title = "Exception",
            StatusMessage = StatusMessage.Error,
            Message = $"Error: {ex.Message}"
        };
    }
}