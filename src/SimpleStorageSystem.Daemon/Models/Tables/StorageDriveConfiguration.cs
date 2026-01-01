namespace SimpleStorageSystem.Daemon.Models.Tables;

public class StorageDriveConfiguration
{
    public long StorageDriveConfigurationId { get; set; }
    public required int MaxConcurrentFileTransfer { get; set; } = 1;

}
