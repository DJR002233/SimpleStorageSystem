namespace SimpleStorageSystem.Daemon.Services.Worker;

public class FileSync
{
    private static readonly Dictionary<Type, List<Action<object>>> Subscriptions = new();
    public FileSync()
    {
        
    }
    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        
    }
}
    