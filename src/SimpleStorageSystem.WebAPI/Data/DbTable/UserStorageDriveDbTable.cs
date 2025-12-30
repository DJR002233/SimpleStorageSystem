using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data.DbTable;

public static class UserStorageDriveDbTable
{
    public static void CreateTableUserStorageDrive(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserStorageDrive>(entity =>
        {
            entity.ToTable("user_storage_drives");
            entity.HasKey(usd => usd.UserStorageDriveId);
            entity.Property(usd => usd.UserStorageDriveId).HasColumnName("user_storage_drive_id");
            entity.Property(usd => usd.StorageNameId).HasColumnName("storage_name_id");
            entity.Property(usd => usd.UserId).HasColumnName("user_id");

            entity.Property(usd => usd.StorageNameId).HasColumnName("storage_name_id");
            entity.HasOne(usd => usd.StorageName)
                .WithMany(sn => sn.Drives)
                .HasForeignKey(usd => usd.StorageNameId)
                .HasPrincipalKey(sn => sn.StorageNameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(usd => usd.UserId).HasColumnName("user_id");
            entity.HasOne(usd => usd.Account)
                .WithMany(a => a.Drives)
                .HasForeignKey(usd => usd.UserId)
                .HasPrincipalKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}