using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Data.TableConfigurations;

public static class StorageDriveTableConfiguration
{
    public static void ConfigureStorageDrivesTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageDrive>(entity =>
        {
            entity.ToTable("storage_drives");
            entity.HasKey(s => s.StorageDriveId);
            entity.Property(s => s.StorageDriveId).HasColumnName("storage_drive_id");
            entity.Property(s => s.Name).HasColumnName("name").HasColumnType("varchar").IsRequired();
            entity.Property(f => f.CreationTime).HasColumnName("creation_time").IsRequired();
            entity.Property(f => f.DeletionTime).HasColumnName("is_deleted");
            entity.Property(f => f.LastSync).HasColumnName("last_sync");
            entity.Property(s => s.Mount).HasColumnName("mount").IsRequired().HasDefaultValue(MountOption.Inactive);
            entity.Property(s => s.StorageNameId).HasColumnName("storage_name_id").IsRequired();

            entity.HasIndex(s => s.Name).IsUnique();
        });
    }
}
