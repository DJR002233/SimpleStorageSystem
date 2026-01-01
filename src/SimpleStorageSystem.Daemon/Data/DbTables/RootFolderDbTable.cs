using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Data.DbTables;

public static class RootFolderDbTable
{
    public static void CreateRootFolderTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RootFolder>(entity =>
        {
            entity.ToTable("root_folders");
            entity.HasKey(rf => rf.RootFolderId);
            entity.Property(rf => rf.RootFolderId).HasColumnName("root_folder_id");
            entity.Property(rf => rf.Name).HasColumnName("name").HasColumnType("varchar").IsRequired();

            entity.Property(rf => rf.MountOption).HasColumnName("mount_option").IsRequired().HasDefaultValue(MountOption.Inactive);
            entity.Property(rf => rf.MirrorFolder).HasColumnName("mirror_folder").IsRequired().HasDefaultValue(false);
            entity.Property(rf => rf.FolderPath).HasColumnName("folder_path").IsRequired().HasColumnType("varchar");

            entity.Property(rf => rf.CreationTime).HasColumnName("creation_time").IsRequired();
            entity.Property(rf => rf.LastModified).HasColumnName("last_modified").IsRequired();
            entity.Property(rf => rf.DeletionTime).HasColumnName("deletion_time");
            entity.Property(rf => rf.LastSync).HasColumnName("last_sync");

            entity.Property(rf => rf.StorageDriveId).HasColumnName("storage_drive_id");

            entity.HasOne(sd => sd.StorageDrive)
                .WithMany(rf => rf.RootFolders)
                .HasForeignKey(f => f.StorageDriveId)
                .HasPrincipalKey(rf => rf.StorageDriveId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

}
