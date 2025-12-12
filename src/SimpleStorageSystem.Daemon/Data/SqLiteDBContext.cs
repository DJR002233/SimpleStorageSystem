using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data.TableConfigurations;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data;

public class SqLiteDbContext : DbContext
{
    public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options) { }

    // Tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.HasDefaultSchema("main");
        modelBuilder.ConfigureFileInformationTable();
        modelBuilder.ConfigureStorageDrivesTable();
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<FileInformation> FileInformation { get; set; }
}

/*
up()
migrationBuilder.Sql(@"
CREATE TRIGGER MountOptionOneMain
BEFORE INSERT OR UPDATE ON MyEntities
BEGIN
    SELECT
        CASE
            WHEN NEW.mount IN (1, 2)
                 AND EXISTS (
                    SELECT 1 FROM MyEntities 
                    WHERE Status IN (1, 2) 
                      AND Id != NEW.Id
                )
            THEN RAISE(ABORT, 'Only one Selected or Selected_Config_1 allowed')
        END;
END;
");

down()
migrationBuilder.Sql("DROP TRIGGER IF EXISTS EnsureSingleSelected;");
*/