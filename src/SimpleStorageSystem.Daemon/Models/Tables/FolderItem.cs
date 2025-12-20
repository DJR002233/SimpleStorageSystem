namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FolderItem : MetaData
{
    public long FolderId { get; set; }
    // public required string FullName { get; set; }
    // public DateTime? CreationTime { get; set; }
    // public DateTime? LastModified { get; set; }
    // public DateTime? LastSync { get; set; }
    public string? MountFolder { get; set; }

    public int? StorageDriveId { get; set; }
    public StorageDrive Drive { get; set; } = null!;
    
}
