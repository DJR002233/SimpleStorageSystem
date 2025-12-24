namespace SimpleStorageSystem.WebAPI.Models.Tables;

public class UserFolder
{
    public long UserFolderId { get; set; }
    public required string FullName { get; set; }
    public DateTime DeletionTime { get; set; }
    public DateTime LastSync { get; set; }

    public long UserStorageDriveId { get; set; }
    public UserStorageDrive StorageDrive { get; set; } = null!;
}
