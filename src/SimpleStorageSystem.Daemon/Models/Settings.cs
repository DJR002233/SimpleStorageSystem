namespace SimpleStorageSystem.Daemon.Models;

public class Settings
{
    public Uri? BaseUri { get; set; }
    public int MaxConcurrentTransfer { get; set; }
    public int MaxConcurrrentConnections { get; set; }
    
}
