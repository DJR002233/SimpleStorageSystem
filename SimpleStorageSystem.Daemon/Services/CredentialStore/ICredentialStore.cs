namespace SimpleStorageSystem.Daemon.Services.CredentialStore;

public interface ICredentialStore
{
    Task StoreAsync(string token);
    Task<string?> GetAsync();
    Task DeleteAsync();
}
