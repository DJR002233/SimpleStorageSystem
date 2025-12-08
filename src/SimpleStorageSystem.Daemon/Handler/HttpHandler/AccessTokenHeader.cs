using System.Net.Http.Headers;
using SimpleStorageSystem.Daemon.Services.Auth.TokenStore;
using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Daemon.Handler.HttpHandler;

public class AccessTokenHeaderHttpHandler : DelegatingHandler
{
    private readonly ITokenStore _tokenStore;

    public AccessTokenHeaderHttpHandler(ITokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ApiResponse<string?> accessToken = await _tokenStore.GetTokenAsync();
        if (!String.IsNullOrEmpty(accessToken.Data))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Data);

        var res = await base.SendAsync(request, cancellationToken);

        return res;
    }
    
}
