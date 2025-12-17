namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FileItem
{
    public long FileId { get; set; }
    public required string Name { get; set; }
    public required string Path { get; set; }
    public string? StartSequence { get; set; }
    public string? MiddleSequence { get; set; }
    public string? EndSequence { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? LastModified { get; set; }
    public DateTime? LastSync { get; set; }
    public int? PendingSyncOperation { get; set; }

    public int StorageDriveId { get; set; }
    public StorageDrive Drive { get; set; } = null!;
}
