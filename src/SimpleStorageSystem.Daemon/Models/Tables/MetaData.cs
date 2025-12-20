namespace SimpleStorageSystem.Daemon.Models.Tables;

public class MetaData
{
    public required string FullName { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? LastModified { get; set; }
    public DateTime? LastSync { get; set; }
}