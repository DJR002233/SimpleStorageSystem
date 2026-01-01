using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data.DbTables;

public static class AppConfigurationDbTable
{
    public static void CreateAppConfigurationTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppConfiguration>( entity =>
        {
            entity.ToTable("app_configurations");
            entity.HasKey(ac => ac.AppConfigurationId);
            entity.Property(ac => ac.AppConfigurationId).HasColumnName("app_configuration_id");
            entity.Property(ac => ac.MaxConcurrentStorageDriveTransfer).HasColumnName("max_concurrent_storage_drive_transfer").IsRequired().HasDefaultValue(1);
            entity.Property(ac => ac.SyncInSeconds).HasColumnName("sync_in_seconds").IsRequired().HasDefaultValue(15);
        });
    }

}
