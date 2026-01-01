namespace SimpleStorageSystem.Daemon.Models.Tables;

public class FolderItem : MetaData
{
    public long FolderId { get; set; }
    public required string RelativePath { get; set; }

    public long RootFolderId { get; set; }
    public RootFolder RootFolder { get; set; } = null!;
    
}
