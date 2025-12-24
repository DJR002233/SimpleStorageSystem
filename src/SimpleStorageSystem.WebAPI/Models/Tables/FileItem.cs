namespace SimpleStorageSystem.WebAPI.Models.Tables;

public class FileItem
{
    public long FileId { get; set; }
    public required string FullName { get; set; }
    public required string Hash { get; set; }
    
    public ICollection<UserFile> UserFiles { get; set; } = new List<UserFile>();
}