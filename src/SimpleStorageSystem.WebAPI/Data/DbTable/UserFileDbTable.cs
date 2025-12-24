using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data.Configurations;

public static class UserFileDbTable
{
    public static void CreateTableUserFile(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserFile>(entity =>
        {
            entity.ToTable("user_files");
            entity.HasKey(uf => uf.UserFileId);
            entity.Property(uf => uf.UserFileId).HasColumnName("user_file_id");
            entity.Property(uf => uf.FullName).HasColumnName("full_name");
            entity.Property(uf => uf.DeletionTime).HasColumnName("deletion_time");
            entity.Property(uf => uf.LastSync).HasColumnName("last_sync");

            entity.Property(uf => uf.FileId).HasColumnName("file_id");
            entity.HasOne(uf => uf.FileItem)
                .WithMany(fi => fi.UserFiles)
                .HasForeignKey(uf => uf.FileId)
                .HasPrincipalKey(fi => fi.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(uf => uf.UserStorageDriveId).HasColumnName("user_storage_drive_id");
            entity.HasOne(uf => uf.StorageDrive)
                .WithMany(usd => usd.Files)
                .HasForeignKey(uf => uf.UserStorageDriveId)
                .HasPrincipalKey(usd => usd.UserStorageDriveId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}