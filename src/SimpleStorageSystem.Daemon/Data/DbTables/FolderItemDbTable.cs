using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data.DbTables;

public static class FolderItemDbTable
{
    public static void CreateFolderItemTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FolderItem>(entity =>
        {
            entity.ToTable("folder_items");
            entity.HasKey(f => f.FolderId);
            entity.Property(f => f.FolderId).HasColumnName("folder_id");
            entity.Property(f => f.RelativePath).HasColumnName("relative_path").HasColumnType("varchar").IsRequired();

            entity.Property(f => f.CreationTime).HasColumnName("creation_time").IsRequired();
            entity.Property(f => f.LastModified).HasColumnName("last_modified").IsRequired();
            entity.Property(f => f.DeletionTime).HasColumnName("deletion_time");
            entity.Property(f => f.LastSync).HasColumnName("last_sync");

            entity.Property(f => f.RootFolderId).HasColumnName("root_folder_id").IsRequired();

            entity.HasOne(f => f.RootFolder)
                .WithMany(rf => rf.Folders)
                .HasForeignKey(f => f.RootFolderId)
                .HasPrincipalKey(rf => rf.RootFolderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}