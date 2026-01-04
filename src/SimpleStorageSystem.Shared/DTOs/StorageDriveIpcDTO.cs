using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Shared.DTOs;

public class StorageDriveIpcDTO
{
    public Guid? StorageDriveId { get; set; }
    public string? Name { get; set; }
    public SupportedStorageServer? StorageServer { get; set; }
    public Uri? BaseAddress { get; set; } = null;

    public MountOption? MountOption { get; set; }
    public bool MirrorDrive { get; set; }
    public string? MountPath { get; set; }
    
    public DateTime CreationTime { get; set; }
    public DateTime? LastSync { get; set; }
}
