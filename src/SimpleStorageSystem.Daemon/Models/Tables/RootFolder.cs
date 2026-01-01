using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Models.Tables;

public class RootFolder : MetaData
{
    public long RootFolderId { get; set; }
    public required string Name { get; set; }
    
    public MountOption MountOption { get; set; } = MountOption.Inactive;
    public required bool MirrorFolder { get; set; } = false;
    public string? FolderPath { get; set; }

    public Guid StorageDriveId { get; set; }
    public StorageDrive StorageDrive { get; set; } = null!;

    public ICollection<FileItem> Files { get; set; } = new List<FileItem>();
    public ICollection<FolderItem> Folders { get; set; } = new List<FolderItem>();
}
