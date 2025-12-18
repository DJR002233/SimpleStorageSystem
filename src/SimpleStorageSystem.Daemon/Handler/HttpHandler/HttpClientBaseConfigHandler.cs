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

        request.RequestUri = new Uri(_settings.BaseUri, request.RequestUri ?? new Uri("", UriKind.Relative));
        
        return await base.SendAsync(request, cancellationToken);
    }

}
