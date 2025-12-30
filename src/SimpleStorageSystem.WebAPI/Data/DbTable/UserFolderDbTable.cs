using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data.DbTable;

public static class UserFolderDbTable
{
    public static void CreateTableUserFolder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserFolder>(entity =>
        {
            entity.ToTable("user_folders");
            entity.HasKey(uf => uf.UserFolderId);
            entity.Property(uf => uf.UserFolderId).HasColumnName("user_file_id");
            entity.Property(uf => uf.FullName).HasColumnName("full_name");
            entity.Property(uf => uf.DeletionTime).HasColumnName("deletion_time");
            entity.Property(uf => uf.LastSync).HasColumnName("last_sync");

            entity.Property(uf => uf.UserStorageDriveId).HasColumnName("user_storage_drive_id");
            entity.HasOne(uf => uf.StorageDrive)
                .WithMany(usd => usd.Folders)
                .HasForeignKey(uf => uf.UserStorageDriveId)
                .HasPrincipalKey(usd => usd.UserStorageDriveId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}