using SimpleStorageSystem.Daemon.Handler.HttpHandler;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class HttpClientCollection
{
    public static IServiceCollection InitializeHttpClientServices(this IServiceCollection services)
    {
        var baseUri = new Uri("http://localhost:5144");
        services.AddHttpClient(HttpClientName.BasicClient.ToString(), client =>
        {
            client.BaseAddress = baseUri;
        })
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient(HttpClientName.TokenClient.ToString(), client =>
        {
            client.BaseAddress = baseUri;
        })
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient(HttpClientName.LogoutClient.ToString(), client =>
        {
            client.BaseAddress = baseUri;
        })
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<AccessTokenHeaderHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient(HttpClientName.AuthenticatedClient.ToString(), client =>
        {
            client.BaseAddress = baseUri;
        })
            .AddHttpMessageHandler<HttpClientBaseConfigHandler>()
            .AddHttpMessageHandler<AccessTokenHeaderHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        return services;
    }
}
