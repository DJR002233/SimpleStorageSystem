using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Models.Tables;

public class StorageDrive
{
    public Guid StorageDriveId { get; set; }
    public required string Name { get; set; }
    public required SupportedStorageServer StorageServer { get; set; }
    public Uri? BaseAddress { get; set; }

    public MountOption MountOption { get; set; } = MountOption.Inactive;
    public bool MirrorDrive { get; set; } = false;
    public string? MountPath { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    public DateTime? LastSync { get; set; }

    public ICollection<RootFolder> RootFolders { get; set; } = new List<RootFolder>();
    
}
