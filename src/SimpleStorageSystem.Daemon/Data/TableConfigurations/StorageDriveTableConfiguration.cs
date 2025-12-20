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
            entity.Property(s => s.Mount).HasColumnName("mount").IsRequired().HasDefaultValue(MountOption.Inactive);

            entity.HasIndex(s => s.Name).IsUnique();
        });
    }
}
