namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FileItem : MetaData
{
    public long FileId { get; set; }
    public required string RelativePath { get; set; }

    public long RootFolderId { get; set; }
    public RootFolder RootFolder { get; set; } = null!;
    
}
