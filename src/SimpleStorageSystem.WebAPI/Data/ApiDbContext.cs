using SimpleStorageSystem.WebAPI.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SimpleStorageSystem.WebAPI.Models.Tables;

namespace SimpleStorageSystem.WebAPI.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

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

public class MyDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
{
    public ApiDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Use the migration connection for design-time tools
        var connectionString = configuration.GetConnectionString("MigrationConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ApiDbContext(optionsBuilder.Options);
    }
}