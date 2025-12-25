using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Shared.Results;

public class StorageDriveResult
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public MountOption Mount { get; set; }
}