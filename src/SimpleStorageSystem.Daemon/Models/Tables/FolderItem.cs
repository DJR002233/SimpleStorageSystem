namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FolderItem
{
    public long FolderId { get; set; }
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required DateTime CreationTime { get; set; }
    public required DateTime LastModified { get; set; }
    public DateTime? LastSync { get; set; }
    public string? MountFolder { get; set; }

    public int? StorageDriveId { get; set; }
    public StorageDrive Drive { get; set; } = null!;
}
