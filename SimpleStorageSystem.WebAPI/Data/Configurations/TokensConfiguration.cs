using SimpleStorageSystem.WebAPI.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace SimpleStorageSystem.WebAPI.Data.Configurations;

public static class TokensConfiguration
{
    public static void ConfigureToken(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("tokens");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).HasColumnName("id");
            entity.Property(b => b.Token).HasColumnName("token").HasColumnType("Text");
            entity.Property(b => b.ExpiresAt).HasColumnName("expires_at");
            entity.Property(b => b.Revoked).HasColumnName("revoked");
            entity.Property(b => b.CreatedAt).HasColumnName("created_at");
            entity.Property(b => b.ReplacedByToken).HasColumnName("replaced_by_token");

            entity.HasOne(a => a.Account)
                .WithMany(b => b.Token)
                .HasForeignKey(b => b.UserId)
                .HasPrincipalKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(b => b.UserId).HasColumnName("user_id");

            entity.Property<uint>("xmin").HasColumnName("xmin").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
        });
    }
}