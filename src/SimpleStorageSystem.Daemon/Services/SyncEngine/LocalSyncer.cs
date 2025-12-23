using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Services.SyncEngine;

public class LocalSyncer
{
    private readonly SqLiteDbContext _dbContext;

    public LocalSyncer(SqLiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Subscriptions()
    {
        Broadcaster.Subscribe<List<(string, ItemType)>>(FileStructureSync);
        Broadcaster.Subscribe<List<(string, ItemType)>>(FolderStructureSync);
    }

    public async ValueTask FileStructureSync(List<(string FullName, ItemType Item)> structure)
    {
        var filesDb = _dbContext.Files;

        // Note to self: replace fileStructure below with new iterative object or replace parameter with HashSet by default. Don't know which is best
        HashSet<string>? fileStructure = structure.Where(file => file.Item == ItemType.File).Select(col => col.FullName).ToHashSet();
        HashSet<string>? dbFullName = filesDb.Select(col => col.FullName).AsNoTracking().ToHashSet();

        foreach (var file in fileStructure)
        {
            FileInfo fileInfo = new FileInfo(file);

            if (!fileInfo.Exists)
                continue;

            if (dbFullName.Contains(file))
            {
                var dbFileItem = filesDb.Local.SingleOrDefault(f => f.FullName == file);
                if(dbFileItem is not null && fileInfo.LastWriteTimeUtc > dbFileItem.LastModified)
                    dbFileItem.LastModified = fileInfo.LastWriteTimeUtc;
                continue;
            }

            filesDb.Add(
                new FileItem
                {
                    FullName = fileInfo.FullName,
                    CreationTime = fileInfo.CreationTimeUtc,
                    LastModified = fileInfo.LastWriteTimeUtc,
                }
            );
        }

        var deletedFiles = filesDb.Where(files => !fileStructure.Contains(files.FullName) && files.DeletionTime == null).AsAsyncEnumerable();

        await foreach(var file in deletedFiles)
        {
            file.DeletionTime = DateTime.UtcNow;
        }
        
        // filesDb.RemoveRange(
        //     filesDb.Local.Where(files => !fileStructure.Contains(files.FullName))
        // );

        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask FolderStructureSync(List<(string FullName, ItemType Item)> structure)
    {
        var foldersDb = _dbContext.Folders;
    }

}
