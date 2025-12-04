namespace SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;

public interface ICredentialStore
{
    Task StoreAsync(string token);
    Task<string?> GetAsync();
    Task DeleteAsync();
}
