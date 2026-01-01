using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Services.Worker;

namespace SimpleStorageSystem.Daemon;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly PipeServer _pipeServer;
    private readonly StorageDriveSyncer _storageDriveSyncer;

    public Worker(
        ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory,
        PipeServer pipeServer, StorageDriveSyncer storageDriveSyncer
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

        _pipeServer = pipeServer;
        _storageDriveSyncer = storageDriveSyncer;

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
            db.Database.EnsureCreated();
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.WhenAll(
                    _pipeServer.ListenAsync(stoppingToken),
                    _storageDriveSyncer.ListenAsync(stoppingToken)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service failed but recovered.");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
