using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data.Configurations;

public static class StorageNameDbTable
{
    public static void CreateTableStorageName(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StorageName>(entity =>
        {
            entity.ToTable("storage_names");
            entity.HasKey(sn => sn.StorageNameId);
            entity.Property(sn => sn.StorageNameId).HasColumnName("storage_name_id");
            entity.Property(sn => sn.Name).HasColumnName("name");
        });
    }
}