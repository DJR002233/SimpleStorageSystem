using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data.TableConfigurations;

public static class FileItemTableConfiguration
{
    public static void ConfigureFileItemTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileItem>(entity =>
        {
            entity.ToTable("file_items");
            entity.HasKey(f => f.FileId);
            entity.Property(f => f.FullName).HasColumnName("full_name").HasColumnType("varchar").IsRequired();

            // entity.Property(f => f.FirstHash).HasColumnName("first_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.SecondHash).HasColumnName("second_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.ThirdHash).HasColumnName("third_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.FourthHash).HasColumnName("fourth_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.FifthHash).HasColumnName("fifth_hash").HasColumnType("varchar").IsRequired();

            entity.Property(f => f.CreationTime).HasColumnName("creation_time").IsRequired();
            entity.Property(f => f.LastModified).HasColumnName("last_modified").IsRequired();
            entity.Property(f => f.LastSync).HasColumnName("last_sync");
            // entity.Property(f => f.PendingSyncOperation).HasColumnName("pending_sync_operation");

            entity.Property(f => f.StorageDriveId).HasColumnName("storage_drive_id");
            
            entity.HasOne(f => f.Drive)
                .WithMany(s => s.Files)
                .HasForeignKey(f => f.StorageDriveId)
                .HasPrincipalKey(s => s.StorageDriveId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
}
