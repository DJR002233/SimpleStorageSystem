using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Services.Worker;
using SimpleStorageSystem.Daemon.Services.Auth;

namespace SimpleStorageSystem.Daemon;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly PipeServer _pipeServer;
    private readonly AuthService _authService;
    private readonly StorageDriveSyncer _syncer;

    public Worker(
        ILogger<Worker> logger, IServiceProvider serviceProvider,
        PipeServer pipeServer, AuthService authService, StorageDriveSyncer syncer)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        _pipeServer = pipeServer;
        _authService = authService;
        _syncer = syncer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        using (var scope = _serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
            db.Database.EnsureCreated();
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("Worker re-ran");

                await _authService.InitializeSessionAsync();

                await Task.WhenAll(
                    _pipeServer.ListenAsync(stoppingToken),
                    _syncer.ListenAsync(stoppingToken)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service failed but recovered.");
            }
        }
    }
}
