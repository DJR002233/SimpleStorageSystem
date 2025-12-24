using SimpleStorageSystem.Daemon.Services;

namespace SimpleStorageSystem.Daemon.Handler.HttpHandler;

public class HttpClientBaseConfigHandler : DelegatingHandler
{
    private readonly SettingsManager _settings;
    
    public HttpClientBaseConfigHandler(SettingsManager settings)
    {
        _settings = settings;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_settings.BaseUri is null) throw new Exception("No uri!");

        var requestUriPath = new Uri("api" + request.RequestUri!.AbsolutePath, UriKind.Relative);

        request.RequestUri = new Uri(_settings.BaseUri, requestUriPath);

        return await base.SendAsync(request, cancellationToken);
    }

}
