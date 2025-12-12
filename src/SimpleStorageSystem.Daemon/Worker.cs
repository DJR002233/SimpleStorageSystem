using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Services.Worker;
using SimpleStorageSystem.Daemon.Services.Auth;

namespace SimpleStorageSystem.Daemon;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly PipeServer _pipeServer;
    private readonly AuthService _authService;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, PipeServer pipeServer, AuthService authService, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _pipeServer = pipeServer;
        _authService = authService;
        _serviceProvider = serviceProvider;
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

                var pipeServer = _pipeServer.ListenAsync(stoppingToken);
                // new Thread(() => {Console.WriteLine();}){IsBackground = true}.Start();

                await Task.WhenAll(pipeServer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service failed but recovered.");
            }
        }
    }
}
