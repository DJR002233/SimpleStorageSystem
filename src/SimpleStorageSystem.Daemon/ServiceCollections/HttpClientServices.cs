using SimpleStorageSystem.Daemon.Handler.HttpHandler;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class HttpClientCollection
{
    public static IServiceCollection InitializeHttpClientServices(this IServiceCollection services)
    {
        var baseUri = new Uri("http://localhost:5144/api/");

        services.AddHttpClient("BasicClient", client => {client.BaseAddress = baseUri;})
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient("TokenClient", client => {client.BaseAddress = baseUri;})
            .AddHttpMessageHandler<RefreshTokenHeaderHttpHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient("LogoutClient", client => {client.BaseAddress = baseUri;})
            .AddHttpMessageHandler<AccessTokenHeaderHttpHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHttpHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient("AuthenticatedClient", client => { client.BaseAddress = baseUri; })
            .AddHttpMessageHandler<AccessTokenHeaderHttpHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        return services;
    }
}
