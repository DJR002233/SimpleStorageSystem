using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data.DbTable;

public static class TokensDbTable
{
    public static void CreateTableToken(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("tokens");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).HasColumnName("id").IsRequired();
            entity.Property(b => b.Token).HasColumnName("token").HasColumnType("Text").IsRequired();
            entity.Property(b => b.ExpiresAt).HasColumnName("expires_at").IsRequired();
            entity.Property(b => b.Revoked).HasColumnName("revoked").IsRequired();
            entity.Property(b => b.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(b => b.ReplacedByToken).HasColumnName("replaced_by_token");

            entity.Property(b => b.UserId).HasColumnName("user_id");
            entity.HasOne(a => a.Account)
                .WithMany(b => b.Token)
                .HasForeignKey(b => b.UserId)
                .HasPrincipalKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property<uint>("xmin").HasColumnName("xmin").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entity.HasIndex(b => b.Token).IsUnique();
        });
    }
}