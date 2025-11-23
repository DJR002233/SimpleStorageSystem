using System;
using System.Net.Http;
using AutoMapper;
using SimpleStorageSystem.AvaloniaDesktop.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.AutoMapperProfiles;

public class ResponseProfile : Profile
{
    public ResponseProfile()
    {
        CreateMap<Response, Response>()
            .AfterMap((src, dest) =>
            {
                string message = "";
                if (!String.IsNullOrWhiteSpace(src.Message))
                    message = src.Message + "\n\n";

                if (src.Errors is not null)
                    foreach (var key in src.Errors)
                    {
                        string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
                        message += $"{combined}\n";
                    }

                dest.Message = message;
            });
        CreateMap(typeof(Response<>), typeof(Response<>))
            .AfterMap((src, dest) =>
            {
                dynamic s = src, d =dest;
                string message = "";
                if (!String.IsNullOrWhiteSpace(s.Message))
                    message = s.Message + "\n\n";

                if (s.Errors is not null)
                    foreach (var key in s.Errors)
                    {
                        string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
                        message += $"{combined}\n";
                    }
                    
                d.Message = message;
            });
        
        CreateMap(typeof(Response<>), typeof(Response))
            .AfterMap((src, dest) =>
            {
                dynamic s = src, d =dest;
                string message = "";
                if (!String.IsNullOrWhiteSpace(s.Message))
                    message = s.Message + "\n\n";

                if (s.Errors is not null)
                    foreach (var key in s.Errors)
                    {
                        string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
                        message += $"{combined}\n";
                    }
                    
                d.Message = message;
            });
        CreateMap(typeof(Response), typeof(Response<>))
            .AfterMap((src, dest) =>
            {
                dynamic s = src, d =dest;
                string message = "";
                if (!String.IsNullOrWhiteSpace(s.Message))
                    message = s.Message + "\n\n";

                if (s.Errors is not null)
                    foreach (var key in s.Errors)
                    {
                        string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
                        message += $"{combined}\n";
                    }
                    
                d.Message = message;
            });

        CreateMap<HttpRequestException, Response>()
            .AfterMap((src, dest) =>
            {
                string message = "";

                if (!String.IsNullOrWhiteSpace(src.StatusCode.ToString()))
                    message += $"Error: {src.StatusCode}\n\n";
                if (!String.IsNullOrWhiteSpace(src.Message))
                    message += src.Message;

                if (String.IsNullOrWhiteSpace(message))
                    message = "Unknown Error!";

                dest.Message = message;
            });
        CreateMap(typeof(HttpRequestException), typeof(Response<>))
            .AfterMap((src, dest) =>
            {
                dynamic s = src, d = dest;
                string message = "";

                if (!String.IsNullOrWhiteSpace(s.StatusCode.ToString()))
                    message += $"Error: {s.StatusCode}\n\n";
                if (!String.IsNullOrWhiteSpace(s.Message))
                    message += s.Message;

                if (String.IsNullOrWhiteSpace(message))
                    message = "Unknown Error!";

                d.Message = message;
            });

        CreateMap<Exception, Response>()
            .AfterMap((src, dest) =>
            {
                dest.Title = "Exception";
                dest.StatusMessage = StatusMessage.Error;
            });
        CreateMap(typeof(Exception), typeof(Response<>))
            .AfterMap((src, dest) =>
            {
                dynamic d = dest;
                d.Title = "Exception";
                d.StatusMessage = StatusMessage.Error;
            });
    }

}