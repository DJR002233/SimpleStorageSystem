using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;

namespace SimpleStorageSystem.Daemon.Handler.HttpHandler;

public class RefreshTokenHeaderHandler : DelegatingHandler
{
    private readonly ICredentialStore _credentialStore;

    public RefreshTokenHeaderHandler(ICredentialStore credentialStore)
    {
        _credentialStore = credentialStore;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? refreshToken = await _credentialStore.GetAsync();
        if (!String.IsNullOrWhiteSpace(refreshToken))
            request.Headers.Add("X-Refresh-Token", refreshToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
