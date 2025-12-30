using SimpleStorageSystem.WebAPI.Data.DbTable;
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

        modelBuilder.CreateTableAccount();
        modelBuilder.CreateTableToken();

        modelBuilder.CreateTableStorageName();
        modelBuilder.CreateTableUserStorageDrive();

        modelBuilder.CreateTableFileItem();
        modelBuilder.CreateTableUserFile();

        modelBuilder.CreateTableUserFolder();

        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<AccountInformation> Accounts { get; set; }
    public DbSet<RefreshToken> Tokens { get; set; }

    public DbSet<StorageName> StorageNames { get; set; }
    public DbSet<UserStorageDrive> StorageDrives { get; set; }

    public DbSet<FileItem> Files { get; set; }
    public DbSet<UserFile> UserFiles { get; set; }

    public DbSet<UserFolder> UserFolders { get; set; }
}

public class ApiDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
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