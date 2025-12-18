using SimpleStorageSystem.Daemon.Handler.HttpHandler;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class HttpClientCollection
{
    public static IServiceCollection InitializeHttpClientServices(this IServiceCollection services)
    {
        services.AddHttpClient("BasicClient")
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient("TokenClient")
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient("LogoutClient")
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<AccessTokenHeaderHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient("AuthenticatedClient")
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<AccessTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        return services;
    }
}
