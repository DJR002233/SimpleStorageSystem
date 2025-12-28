using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Daemon.Services.Auth;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Services.Worker;

public class StorageDriveSyncer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AuthService _authService;
    private readonly List<(string, ItemType)> _directoryStructure = new();

    public StorageDriveSyncer(IServiceScopeFactory serviceScopeFactory, AuthService authService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _authService = authService;
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_authService.HasSession())
                {
                    await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                    continue;
                }

                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

                var mainDrive = await dbContext.Drives.SingleOrDefaultAsync(
                    d => d.Mount == MountOption.MainOnDrive ||
                    d.Mount == MountOption.MainOnServer
                );

                foreach (var folder in mainDrive?.Folders ?? Enumerable.Empty<FolderItem>())
                {
                    await DirectoryRecursiver(folder.FullName);
                }

                Console.WriteLine("\n\nFINISHED SCANNING DIRECTORY!\n\n");

                await Broadcaster.PublishInParallelAsync(_directoryStructure);

                ClearStructureList();

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
        catch (OperationCanceledException) //when (stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("StorageDriveSyncer service worker operation cancelled...");
        }


    }

    public async Task DirectoryRecursiver(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        var subDirs = dir.EnumerateDirectories();

        Console.WriteLine($"FOLDER: {path}");

        foreach (var subDir in subDirs)
        {
            await DirectoryRecursiver(subDir.FullName);

            // FolderItem folderItem = new FolderItem
            // {
            //     FullName = dir.FullName,
            //     CreationTime = dir.CreationTimeUtc,
            //     LastModified = dir.LastWriteTimeUtc,
            // };
            _directoryStructure.Add((subDir.FullName, ItemType.Folder));
        }

        var files = dir.EnumerateFiles();
        foreach (var file in files)
        {
            SyncFile(file);
        }
    }

    public void SyncFile(FileInfo file)
    {
        // FileItem fileItem = new FileItem
        // {
        //     FullName = file.FullName,
        //     CreationTime = file.CreationTimeUtc,
        //     LastModified = file.LastWriteTimeUtc,
        // };
        Console.WriteLine($"FILE: {file.FullName}");
        _directoryStructure.Add((file.FullName, ItemType.File));
    }

    public void ClearStructureList()
    {
        _directoryStructure.Clear();
        _directoryStructure.TrimExcess();
    }

}
