using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data.DbTable;

public static class FileItemDbTable
{
    public static void CreateTableFileItem(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileItem>(entity =>
        {
            entity.ToTable("file_items");
            entity.HasKey(fi => fi.FileId);
            entity.Property(fi => fi.FileId).HasColumnName("file_id");
            entity.Property(fi => fi.FullName).HasColumnName("full_name");
            entity.Property(fi => fi.Hash).HasColumnName("hash");
            entity.Property(fi => fi.Size).HasColumnName("size");
        });
    }
}