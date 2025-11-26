using Microsoft.Extensions.DependencyInjection;
using System;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;
using SimpleStorageSystem.AvaloniaDesktop.Handler.HttpHandler;
using SimpleStorageSystem.AvaloniaDesktop.Services.TokenStore;

namespace SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;

public static class HttpClientCollection
{
    public static IServiceCollection InitializeHttpClientServices(this IServiceCollection services)
    {
        var baseUri = new Uri("http://localhost:5144/api/");

        services.AddHttpClient<ISessionManager, SessionManager>(client => {client.BaseAddress = baseUri;})
            .AddHttpMessageHandler<AccessTokenHeaderHttpHandler>()
            .AddHttpMessageHandler<RefreshTokenHeaderHttpHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient<ITokenStore, TokenStore>(client => {client.BaseAddress = baseUri;})
            .AddHttpMessageHandler<RefreshTokenHeaderHttpHandler>()
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        services.AddHttpClient<AuthService>( client => { client.BaseAddress = baseUri; })
            .AddHttpMessageHandler<HttpSocketExceptionHandler>();

        return services;
    }
}
        