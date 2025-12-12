namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FolderItem
{
    public long FolderId { get; set; }
    public required string[] Path { get; set; }
    public DateTime? LastSync { get; set; }
    public int? PendingSyncOperation { get; set; }
    public string? MountFolder { get; set; }

    public int StorageDriveId { get; set; }
    public StorageDrive drive { get; set; } = null!;
}
