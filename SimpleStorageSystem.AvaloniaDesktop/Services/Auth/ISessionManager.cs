using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public interface ISessionManager
{
    Task<string?> GetAccessTokenAsync();
    string? GetRefreshToken();
    Task<bool> SetSessionAsync(Session session);
    Task<Response> InitializeSessionAsync();
    Task<Response> TerminateSessionAsync();
}