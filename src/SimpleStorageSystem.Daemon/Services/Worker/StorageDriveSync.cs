using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Services.Worker;

public class DriveSync
{
    private readonly SqLiteDbContext _dbContext;
    private readonly List<FileItem> _fileStructure = new();
    private readonly List<FolderItem> _folderStructure = new();

    public DriveSync(SqLiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var mainDrive = await _dbContext.Drive.SingleOrDefaultAsync(
                d => d.Mount == Shared.Enums.MountOption.MAIN_ON_DRIVE ||
                d.Mount == Shared.Enums.MountOption.MAIN_ON_SERVER
            );

            foreach (var folder in mainDrive?.Folders!)
            {
                await DirectoryRecursiver(folder.Path);
            }

            await Broadcaster.PublishInOrder(_fileStructure);
            await Broadcaster.PublishInOrder(_folderStructure);

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

            FolderItem folderItem = new FolderItem
            {
                Name = dir.Name,
                Path = dir.FullName,
                CreationTime = dir.CreationTimeUtc,
                LastModified = dir.LastWriteTimeUtc,
            };
            _folderStructure.Add(folderItem);
        }

        var files = dir.EnumerateFiles();
        foreach (var file in files)
        {
            await SyncFile(file);
        }
    }

    public async Task SyncFile(FileInfo file)
    {
        FileItem fileItem = new FileItem
        {
            Name = file.Name,
            Path = file.FullName,
            CreationTime = file.CreationTimeUtc,
            LastModified = file.LastWriteTimeUtc,
        };
        _fileStructure.Add(fileItem);
    }

    public void ClearStructureList()
    {
        _fileStructure.Clear();
        _folderStructure.Clear();

        _fileStructure.TrimExcess();
        _folderStructure.TrimExcess();
    }

}
