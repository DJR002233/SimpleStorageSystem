using AutoMapper;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Shared.AutoMapperProfiles;

public class ApiResponseProfile : Profile
{
    public ApiResponseProfile()
    {
        CreateMap<ApiResponse, ApiResponse>()
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
        CreateMap(typeof(ApiResponse<>), typeof(ApiResponse<>))
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
        
        CreateMap(typeof(ApiResponse<>), typeof(ApiResponse))
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
        CreateMap(typeof(ApiResponse), typeof(ApiResponse<>))
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

        CreateMap<HttpRequestException, ApiResponse>()
            .AfterMap((src, dest) =>
            {
                string message = "";

                if (!String.IsNullOrWhiteSpace(src.StatusCode.ToString()))
                    message += $"Error: {src.StatusCode}\n\n";
                if (!String.IsNullOrWhiteSpace(src.Message))
                    message += src.Message;

                if (String.IsNullOrWhiteSpace(message))
                    message = "Unknown Error!";

                dest.Title = src.GetType().ToString();
                dest.StatusMessage = ApiStatus.Error;
                dest.Message = message;
            });
        CreateMap(typeof(HttpRequestException), typeof(ApiResponse<>))
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

                d.Title = src.GetType().ToString();
                d.StatusMessage = ApiStatus.Error;
                d.Message = message;
            });

        CreateMap<Exception, ApiResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Title = src.GetType().ToString();
                dest.StatusMessage = ApiStatus.Error;
            });
        CreateMap(typeof(Exception), typeof(ApiResponse<>))
            .AfterMap((src, dest) =>
            {
                dynamic d = dest;
                d.Title = src.GetType().ToString();
                d.StatusMessage = ApiStatus.Error;
            });
            
    }

}
