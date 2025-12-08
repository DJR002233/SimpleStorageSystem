
using SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;
using SimpleStorageSystem.Daemon;
using SimpleStorageSystem.Daemon.ServiceCollections;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.InitializeBaseServices();
builder.Services.InitializeHttpClientServices();
builder.Services.InitializeIpcCommandHandlerServices();

var host = builder.Build();
host.Run();
