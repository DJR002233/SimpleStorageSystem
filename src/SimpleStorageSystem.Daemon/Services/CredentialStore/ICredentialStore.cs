namespace SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;

public interface ICredentialStore
{
    ValueTask StoreAsync(string password, string value);
    ValueTask<string?> GetAsync(string value);
    ValueTask DeleteAsync(string value);
}
