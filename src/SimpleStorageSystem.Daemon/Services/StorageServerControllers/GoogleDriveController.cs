using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;
using SimpleStorageSystem.Shared.Enums;

namespace SimpleStorageSystem.Daemon.Services.StorageServerControllers;

public class GoogleDriveController : IStorageServerController
{
    private readonly SqLiteDbContext _dbContext;
    private readonly ICredentialStore _credentialStore;

    public SupportedStorageServer StorageServer => SupportedStorageServer.GoogleDrive;

    public GoogleDriveController(SqLiteDbContext dbContext, ICredentialStore credentialStore)
    {
        _dbContext = dbContext;
        _credentialStore = credentialStore;
    }

    public async ValueTask LoginAsync(string name)
    {
        Guid guid = Guid.NewGuid();

        UserCredential credential;

        string[] scopes =
        {
            DriveService.Scope.Drive,
            // DriveService.Scope.DriveFile,
        };
        // string tokenPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"SimpleStorageSystem/{guid}.json");

        using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
        {
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                scopes,
                guid.ToString(),
                CancellationToken.None
            );
        }

        await _credentialStore.StoreAsync(JsonSerializer.Serialize(credential.Token), guid.ToString());
        _dbContext.StorageDrives.Add(new StorageDrive { StorageDriveId = guid, Name = name, StorageServer = StorageServer });

        await _dbContext.SaveChangesAsync();
    }

}
