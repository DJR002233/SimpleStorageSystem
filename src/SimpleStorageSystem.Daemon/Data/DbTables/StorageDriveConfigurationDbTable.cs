using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data.DbTables;

public static class StorageDriveConfigurationDbTable
{
    public static void CreateStorageDriveConfigurationTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageDriveConfiguration>( entity =>
        {
            entity.ToTable("storage_drive_configurations");
            entity.HasKey(sdc => sdc.StorageDriveConfigurationId);
            entity.Property(sdc => sdc.StorageDriveConfigurationId).HasColumnName("storage_drive_configuration_id");
            entity.Property(sdc => sdc.MaxConcurrentFileTransfer).HasColumnName("max_concurrent_file_transfer").IsRequired().HasDefaultValue(1);
        });
    }

}
