using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
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
        var accessToken = await _sessionManager.GetAccessTokenAsync();

        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var res = await base.SendAsync(request, cancellationToken);

        return res;
    }
}
