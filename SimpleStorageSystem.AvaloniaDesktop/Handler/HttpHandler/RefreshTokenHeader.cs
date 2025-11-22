using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler.HttpHandler;

public class RefreshTokenHeaderHttpHandler : DelegatingHandler
{
    private readonly ISessionManager _sessionManager;

    public RefreshTokenHeaderHttpHandler(SessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var refreshToken = _sessionManager.GetRefreshToken();

        if (!string.IsNullOrEmpty(refreshToken))
            request.Headers.Add("X-Refresh-Token", refreshToken);

        var res = await base.SendAsync(request, cancellationToken);

        return res;
    }
}
