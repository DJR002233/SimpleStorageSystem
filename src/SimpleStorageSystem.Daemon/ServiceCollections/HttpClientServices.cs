using SimpleStorageSystem.Daemon.Handler.HttpHandler;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class HttpClientCollection
{
    public static IServiceCollection InitializeHttpClientServices(this IServiceCollection services)
    {
        services.AddHttpClient(HttpClientName.BasicClient.ToString())
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient(HttpClientName.TokenClient.ToString())
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient(HttpClientName.LogoutClient.ToString())
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<AccessTokenHeaderHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient(HttpClientName.AuthenticatedClient.ToString())
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<AccessTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        return services;
    }
}
