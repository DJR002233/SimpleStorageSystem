using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data.TableConfigurations;

public static class FileInformationTableConfiguration
{
    public static void ConfigureFileInformationTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileInformation>(entity =>
        {
            entity.ToTable("file_informations");
            entity.HasKey(f => f.FileId);
            entity.Property(f => f.Path).HasColumnName("path").HasColumnType("varchar").IsRequired();
            entity.Property(f => f.LastModified).HasColumnName("last_modified").IsRequired();
            entity.Property(f => f.Hash).HasColumnName("hash").HasColumnType("varchar").IsRequired();
            entity.Property(f => f.LastSync).HasColumnName("last_sync");
            entity.Property(f => f.PendingSyncOperation).HasColumnName("pending_sync_operation");
            entity.Property(f => f.MountFolder).HasColumnName("mount_folder");

            entity.Property(f => f.StorageDriveId).HasColumnName("storage_drive_id");
            
            entity.HasOne(f => f.drive)
                .WithMany(s => s.Files)
                .HasForeignKey(f => f.StorageDriveId)
                .HasPrincipalKey(s => s.StorageDriveId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}