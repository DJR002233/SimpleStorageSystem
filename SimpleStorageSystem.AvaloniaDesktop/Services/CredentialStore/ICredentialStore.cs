using System.Threading.Tasks;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.CredentialStore;

public interface ICredentialStore
{
    Task StoreAsync(string token);
    Task<string?> GetAsync();
    Task DeleteAsync();
}
