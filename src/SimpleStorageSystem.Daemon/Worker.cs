using SimpleStorageSystem.Daemon.Services;
using SimpleStorageSystem.Daemon.Services.Auth;

namespace SimpleStorageSystem.Daemon;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly PipeServer _authPipe;
    private readonly AuthService _authService;

    public Worker(ILogger<Worker> logger, PipeServer authPipe, AuthService authService)
    {
        _logger = logger;
        _authPipe = authPipe;
        _authService = authService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }
        await _authService.InitializeSessionAsync();

        await Task.WhenAll(_authPipe.ListenAsync(stoppingToken));
    }
}
