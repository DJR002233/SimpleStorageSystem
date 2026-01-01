using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Shared.Enums;
// using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Data.DbTables;

public static class StorageDriveDbTables
{
    public static void CreateStorageDriveTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageDrive>(entity =>
        {
            entity.ToTable("storage_drives");
            entity.HasKey(sd => sd.StorageDriveId);
            entity.Property(sd => sd.StorageDriveId).HasColumnName("storage_drive_id");
            entity.Property(sd => sd.Name).HasColumnName("name").HasColumnType("varchar").IsRequired();
            entity.Property(sd => sd.StorageServer).HasColumnName("storage_server").IsRequired();
            entity.Property(sd => sd.BaseAddress).HasColumnName("base_address");

            entity.Property(sd => sd.MountOption).HasColumnName("mount_option").IsRequired().HasDefaultValue(MountOption.Inactive);
            entity.Property(sd => sd.MirrorDrive).HasColumnName("mirror_drive").IsRequired().HasDefaultValue(false);
            entity.Property(sd => sd.MountPath).HasColumnName("mount_path");

            entity.Property(f => f.CreationTime).HasColumnName("creation_time").IsRequired().HasDefaultValue(DateTime.UtcNow);
            entity.Property(f => f.LastSync).HasColumnName("last_sync");
        });
    }

}
