using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data.DbTables;

public static class FileItemDbTable
{
    public static void CreateFileItemTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileItem>(entity =>
        {
            entity.ToTable("file_items");
            entity.HasKey(f => f.FileId);
            entity.Property(f => f.FileId).HasColumnName("file_id");
            entity.Property(f => f.RelativePath).HasColumnName("relative_path").HasColumnType("varchar").IsRequired();

            // entity.Property(f => f.FirstHash).HasColumnName("first_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.SecondHash).HasColumnName("second_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.ThirdHash).HasColumnName("third_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.FourthHash).HasColumnName("fourth_hash").HasColumnType("varchar").IsRequired();
            // entity.Property(f => f.FifthHash).HasColumnName("fifth_hash").HasColumnType("varchar").IsRequired();

            entity.Property(f => f.CreationTime).HasColumnName("creation_time").IsRequired();
            entity.Property(f => f.LastModified).HasColumnName("last_modified").IsRequired();
            entity.Property(f => f.DeletionTime).HasColumnName("deletion_time");
            entity.Property(f => f.LastSync).HasColumnName("last_sync");

            entity.Property(f => f.RootFolderId).HasColumnName("root_folder_id");

            entity.HasOne(f => f.RootFolder)
                .WithMany(rf => rf.Files)
                .HasForeignKey(f => f.RootFolderId)
                .HasPrincipalKey(rf => rf.RootFolderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

}
