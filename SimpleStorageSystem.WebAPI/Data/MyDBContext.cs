using SimpleStorageSystem.WebAPI.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.WebAPI.Models.Auth;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleStorageSystem.WebAPI.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    // Tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("main");
        modelBuilder.ConfigureAccount();
        modelBuilder.ConfigureToken();
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<AccountInformation> Accounts { get; set; }
    public DbSet<RefreshToken> Tokens { get; set; }
}

public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Use the migration connection for design-time tools
        var connectionString = configuration.GetConnectionString("MigrationConnection");

        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new MyDbContext(optionsBuilder.Options);
    }
}