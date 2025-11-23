using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler.HttpHandler;

public class AccessTokenHeaderHttpHandler : DelegatingHandler
{
    private readonly ISessionManager _sessionManager;

    public AccessTokenHeaderHttpHandler(ISessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Response<string?> accessToken = await _sessionManager.GetAccessTokenAsync();

        if (!string.IsNullOrEmpty(accessToken.Data))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Data);

        var res = await base.SendAsync(request, cancellationToken);

        return res;
    }
}
