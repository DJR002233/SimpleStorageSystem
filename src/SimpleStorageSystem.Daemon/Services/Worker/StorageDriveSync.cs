using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Services.Worker;

public class DriveSync
{
    private readonly SqLiteDbContext _dbContext;
    private readonly List<(string, ItemType)> _directoryStructure = new();

    public DriveSync(SqLiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var mainDrive = await _dbContext.Drives.SingleOrDefaultAsync(
                d => d.Mount == MountOption.MAIN_ON_DRIVE ||
                d.Mount == MountOption.MAIN_ON_SERVER
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
            _directoryStructure.Add((subDir.FullName,ItemType.FOLDER));
        }

        var files = dir.EnumerateFiles();
        foreach (var file in files)
        {
            await SyncFile(file);
        }
    }

    public async Task SyncFile(FileInfo file)
    {
        // FileItem fileItem = new FileItem
        // {
        //     FullName = file.FullName,
        //     CreationTime = file.CreationTimeUtc,
        //     LastModified = file.LastWriteTimeUtc,
        // };
        _directoryStructure.Add((file.FullName, ItemType.FILE));
    }

    public void ClearStructureList()
    {
        _directoryStructure.Clear();
        _directoryStructure.TrimExcess();
    }

}
