using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleStorageSystem.AvaloniaDesktop.Handler.HttpHandler;

public class RefreshTokenHeaderHttpHandler : DelegatingHandler
{
    private readonly Session _session;

    public RefreshTokenHeaderHttpHandler(Session session)
    {
        _session = session;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var refreshToken = _session.RefreshToken;

        if (!string.IsNullOrEmpty(refreshToken))
            request.Headers.Add("X-Refresh-Token", refreshToken);

        var res = await base.SendAsync(request, cancellationToken);

        return res;
    }
}
