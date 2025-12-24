using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Services.Worker;

public class StorageDriveSyncer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly List<(string, ItemType)> _directoryStructure = new();

    public StorageDriveSyncer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

            var mainDrive = await dbContext.Drives.SingleOrDefaultAsync(
                d => d.Mount == MountOption.MainOnDrive ||
                d.Mount == MountOption.MainOnServer
            );

            foreach (var folder in mainDrive?.Folders!)
            {
                await DirectoryRecursiver(folder.FullName);
            }

            await Broadcaster.PublishInParallelAsync(_directoryStructure);

            ClearStructureList();

            await Task.Delay(15 * 1000);
        }

    }

    public async Task DirectoryRecursiver(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        var subDirs = dir.EnumerateDirectories();

        foreach (var subDir in subDirs)
        {
            await DirectoryRecursiver(subDir.FullName);

            // FolderItem folderItem = new FolderItem
            // {
            //     FullName = dir.FullName,
            //     CreationTime = dir.CreationTimeUtc,
            //     LastModified = dir.LastWriteTimeUtc,
            // };
            _directoryStructure.Add((subDir.FullName,ItemType.Folder));
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
        _directoryStructure.Add((file.FullName, ItemType.File));
    }

    public void ClearStructureList()
    {
        _directoryStructure.Clear();
        _directoryStructure.TrimExcess();
    }

}
