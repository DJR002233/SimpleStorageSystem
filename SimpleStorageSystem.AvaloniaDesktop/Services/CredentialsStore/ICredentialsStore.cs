using System.Threading.Tasks;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.CredentialsStore;

public interface ICredentialsStore
{
    Task StoreAsync(string token);
    Task<string?> GetAsync();
    Task DeleteAsync();
}
