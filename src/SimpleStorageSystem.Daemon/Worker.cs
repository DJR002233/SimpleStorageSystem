using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Services.Worker;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Daemon.Services;

namespace SimpleStorageSystem.Daemon;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly PipeServer _pipeServer;
    private readonly AuthService _authService;
    private readonly StorageDriveSyncer _syncer;

    public Worker(
        ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory,
        PipeServer pipeServer, AuthService authService, StorageDriveSyncer syncer,
        SettingsManager settingsManager)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

        _pipeServer = pipeServer;
        _authService = authService;
        _syncer = syncer;

        settingsManager.SetBaseUri("http://localhost:5144/api");

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
                await _authService!.InitializeSessionAsync();
                
                await Task.WhenAll(
                    _pipeServer.ListenAsync(stoppingToken),
                    _syncer.ListenAsync(stoppingToken)
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
