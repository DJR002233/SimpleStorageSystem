using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Models.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.TokenStore;

public interface ITokenStore
{
    string? RefreshToken { get; set; }
    Task<bool> SetSessionAsync(Session session);
    Task<Response<string?>> GetAccessTokenAsync();
}