namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FileItem
{
    public long FileId { get; set; }
    public required string FullName { get; set; }
    // public string? FirstHash { get; set; }
    // public string? SecondHash { get; set; }
    // public string? ThirdHash { get; set; }
    // public string? FourthHash { get; set; }
    // public string? FifthHash { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? LastModified { get; set; }
    public DateTime? LastSync { get; set; }
    public int? PendingSyncOperation { get; set; }

    public int StorageDriveId { get; set; }
    public StorageDrive Drive { get; set; } = null!;
    
}
