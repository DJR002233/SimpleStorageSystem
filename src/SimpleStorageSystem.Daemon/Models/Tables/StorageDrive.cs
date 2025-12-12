using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Models.Tables;

public class StorageDrive
{
    public int StorageDriveId { get; set; }
    public required string Name { get; set; }
    public MountOption Mount { get; set; }
    
    public ICollection<FileInformation> Files { get; set; } = new List<FileInformation>();
}
