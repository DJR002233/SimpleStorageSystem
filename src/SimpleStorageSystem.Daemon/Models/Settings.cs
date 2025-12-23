namespace SimpleStorageSystem.Daemon.Models;

public class Settings
{
    public Uri? BaseUri { get; set; }
    public int MaxConcurrentTransfer { get; set; } = 1;
    public int MaxConcurrrentConnections { get; set; } = 1;
    public int SyncInSeconds { get; set; } = 15;

}
