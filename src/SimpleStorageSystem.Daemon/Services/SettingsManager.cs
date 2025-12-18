using SimpleStorageSystem.Daemon.Models;

namespace SimpleStorageSystem.Daemon.Services;

public class SettingsManager
{
    private readonly Settings _settings;

    public Uri? BaseUri => _settings.BaseUri;
    public int MaxConcurrentTransfer
    {
        get => _settings.MaxConcurrentTransfer;
        set => _settings.MaxConcurrentTransfer = value;
    }
    public int MaxConcurrentConnections{
        get => _settings.MaxConcurrrentConnections;
        set => _settings.MaxConcurrrentConnections = value;
    }

    public SettingsManager()
    {
        _settings = new Settings();
    }

    public void SetBaseUri(string uri)
    {
        Uri baseUri = new Uri(uri, UriKind.Absolute);
        _settings.BaseUri = baseUri;
    }

}
