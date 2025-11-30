using SimpleStorageSystem.Daemon.Models;

namespace SimpleStorageSystem.Daemon.Services.TokenStore;

public interface ITokenStore
{
    string? RefreshToken { get; set; }
    Task<bool> SetSessionAsync(Session session);
    void ClearSession();
    Task<Response<string?>> GetAccessTokenAsync();
}