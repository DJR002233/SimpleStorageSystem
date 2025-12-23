using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Models.Tables;

public class StorageDrive
{
    public long StorageDriveId { get; set; }
    public required string Name { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? DeletionTime { get; set; }
    public DateTime? LastSync { get; set; }
    public MountOption Mount { get; set; }
    public long StorageNameId { get; set; }

    public ICollection<FileItem> Files { get; set; } = new List<FileItem>();
    public ICollection<FolderItem> Folders { get; set; } = new List<FolderItem>();
}
