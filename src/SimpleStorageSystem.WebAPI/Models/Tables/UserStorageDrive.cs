namespace SimpleStorageSystem.WebAPI.Models.Tables;

public class UserStorageDrive
{
    public long UserStorageDriveId { get; set; }
    public long StorageNameId { get; set; }
    public Guid UserId { get; set; }

    public StorageName StorageName { get; set; } = null!;
    public AccountInformation Account { get; set; } = null!;
    
    public ICollection<UserFile> Files { get; set; } = new List<UserFile>();
    public ICollection<UserFolder> Folders { get; set; } = new List<UserFolder>();
}
