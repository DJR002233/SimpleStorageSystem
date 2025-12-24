using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data.TableConfigurations;

public static class FolderItemTableConfiguration
{
    public static void ConfigureFolderItemTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FolderItem>(entity =>
        {
            entity.ToTable("folder_items");
            entity.HasKey(f => f.FolderId);
            entity.Property(f => f.FullName).HasColumnName("full_name").HasColumnType("varchar").IsRequired();
            entity.Property(f => f.CreationTime).HasColumnName("creation_time").IsRequired();
            entity.Property(f => f.LastModified).HasColumnName("last_modified").IsRequired();
            entity.Property(f => f.LastSync).HasColumnName("last_sync");
            entity.Property(f => f.DeletionTime).HasColumnName("deletion_time");
            entity.Property(f => f.MountFolder).HasColumnName("mount_folder");
            entity.Property(f => f.IsRootFolder).HasColumnName("is_root_folder").HasDefaultValue(false);

            entity.Property(f => f.StorageDriveId).HasColumnName("storage_drive_id");
            
            entity.HasOne(f => f.Drive)
                .WithMany(s => s.Folders)
                .HasForeignKey(f => f.StorageDriveId)
                .HasPrincipalKey(s => s.StorageDriveId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}