namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FileItem
{
    public long FileId { get; set; }
    public required string Path { get; set; }
    public required DateTime LastModified { get; set; }
    public required string Hash { get; set; }
    public DateTime? LastSync { get; set; }
    public int? PendingSyncOperation { get; set; }

    public int StorageDriveId { get; set; }
    public StorageDrive drive { get; set; } = null!;
}
