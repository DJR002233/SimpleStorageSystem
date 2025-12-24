using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data.Configurations;

public static class AccountDbTable
{
    public static void CreateTableAccount(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountInformation>(entity =>
        {
            entity.ToTable("accounts");
            entity.HasKey(a => a.UserId);
            entity.Property(a => a.UserId).HasColumnName("user_id").IsRequired();
            // entity.Property(a => a.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(a => a.Username).HasColumnName("username").HasColumnType("varchar").HasMaxLength(254).IsRequired();
            entity.Property(a => a.Email).HasColumnName("email").HasColumnType("varchar").HasMaxLength(254).IsRequired();
            entity.Property(a => a.Password).HasColumnName("password").HasColumnType("bytea").IsRequired();
            entity.Property(a => a.CreatedAt).HasColumnName("created_at").IsRequired();
            
            // entity.HasIndex(a => a.Id).IsUnique();
        });
    }
}