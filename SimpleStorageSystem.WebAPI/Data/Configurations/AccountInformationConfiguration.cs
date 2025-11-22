using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Auth;

namespace SimpleStorageSystem.WebAPI.Data.Configurations;

public static class AccountInformationConfiguration
{
    public static void ConfigureAccount(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountInformation>(entity =>
        {
            entity.ToTable("accounts");
            entity.HasKey(a => a.UserId);
            entity.Property(a => a.UserId).HasColumnName("user_id");
            entity.Property(a => a.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(a => a.Username).IsRequired().HasMaxLength(254).HasColumnType("varchar").HasColumnName("username");
            entity.Property(a => a.Email).IsRequired().HasMaxLength(254).HasColumnType("varchar").HasColumnName("email");
            entity.Property(a => a.Password).IsRequired().HasColumnType("bytea").HasColumnName("password");
            
            entity.HasIndex(a => a.Id).IsUnique();
        });
    }
}