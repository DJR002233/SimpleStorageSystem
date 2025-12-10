using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.AvaloniaDesktop.Data.TableConfigurations;
using SimpleStorageSystem.AvaloniaDesktop.Models.Tables;

namespace SimpleStorageSystem.AvaloniaDesktop.Data;

public class SqLiteDbContext : DbContext
{
    public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options) { }

    // Tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("main");
        modelBuilder.ConfigureFileInformationTable();
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<FileInformation> FileInformation { get; set; }
}

// public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
// {
//     public MyDbContext CreateDbContext(string[] args)
//     {
//         var configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json")
//             .Build();

//         // Use the migration connection for design-time tools
//         var connectionString = configuration.GetConnectionString("MigrationConnection");

//         var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
//         optionsBuilder.UseNpgsql(connectionString);

//         return new MyDbContext(optionsBuilder.Options);
//     }
// }