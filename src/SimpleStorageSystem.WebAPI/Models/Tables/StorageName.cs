namespace SimpleStorageSystem.WebAPI.Models.Tables;

public class StorageName
{
    public long StorageNameId { get; set; }
    public required string Name { get; set; }

    public ICollection<UserStorageDrive> Drives { get; set; } = new List<UserStorageDrive>();
}