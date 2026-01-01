using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data.DbTables;
using SimpleStorageSystem.Daemon.Models.Tables;

namespace SimpleStorageSystem.Daemon.Data;

public class SqLiteDbContext : DbContext
{
    public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options) { }

    // Tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.HasDefaultSchema("main");
        modelBuilder.CreateStorageDriveTable();
        modelBuilder.CreateRootFolderTable();
        modelBuilder.CreateFileItemTable();
        modelBuilder.CreateFolderItemTable();

        modelBuilder.CreateAppConfigurationTable();
        modelBuilder.CreateStorageDriveConfigurationTable();
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<FileItem> Files { get; set; }
    public DbSet<FolderItem> Folders { get; set; }
    public DbSet<RootFolder> RootFolders { get; set; }
    public DbSet<StorageDrive> StorageDrives { get; set; }

    public DbSet<AppConfiguration> AppConfigurations { get; set; }
    public DbSet<StorageDriveConfiguration> StorageDriveConfigurations { get; set; }
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
