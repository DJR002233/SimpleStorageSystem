using System.Collections;
using System.Collections.Generic;

namespace SimpleStorageSystem.AvaloniaDesktop.Models.Tables;

public class StorageDrive
{
    public int StorageDriveId { get; set; }
    public required string Name { get; set; }
    
    public ICollection<FileInformation> Files { get; set; } = new List<FileInformation>();
}
