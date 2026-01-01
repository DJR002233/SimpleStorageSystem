namespace SimpleStorageSystem.Shared.DTOs;

public class RootFolderIpcDTO
{
    public long RootFolderId { get; set; }
    public string? Name { get; set; }
    public bool? MirrorFolder { get; set; } = false;
    public string? FolderPath { get; set; }

}
