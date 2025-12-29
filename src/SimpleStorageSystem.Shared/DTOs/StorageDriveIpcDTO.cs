using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Shared.DTOs;

public class StorageDriveIpcDTO
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public MountOption Mount { get; set; }
}
