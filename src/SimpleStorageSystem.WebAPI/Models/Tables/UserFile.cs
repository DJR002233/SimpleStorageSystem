namespace SimpleStorageSystem.WebAPI.Models.Tables;

public class UserFile
{
    public long UserFileId { get; set; }
    public required string FullName { get; set; }
    public DateTime DeletionTime { get; set; }
    public DateTime LastSync { get; set; }

    public long FileId { get; set; }
    public long UserStorageDriveId { get; set; }

    public FileItem FileItem { get; set; } = null!;
    public UserStorageDrive StorageDrive { get; set; } = null!;
}
