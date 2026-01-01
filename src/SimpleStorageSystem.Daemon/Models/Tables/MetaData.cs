namespace SimpleStorageSystem.Daemon.Models.Tables;

public class MetaData
{
    public DateTime? CreationTime { get; set; }
    public DateTime? LastModified { get; set; }
    public DateTime? DeletionTime { get; set; }
    public DateTime? LastSync { get; set; }
}