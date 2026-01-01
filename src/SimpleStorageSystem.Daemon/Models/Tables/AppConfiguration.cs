namespace SimpleStorageSystem.Daemon.Models.Tables;

public class AppConfiguration
{
    public long AppConfigurationId { get; set; }
    public required int MaxConcurrentStorageDriveTransfer { get; set; } = 1;
    public required int SyncInSeconds { get; set; } = 15;

}
