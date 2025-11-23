using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public interface ISessionManager
{
    Task<Response<string?>> GetAccessTokenAsync();
    string? GetRefreshToken();
    Task<bool> SetSessionAsync(Session session);
    Task<Response> InitializeSessionAsync();
    Task<Response> TerminateSessionAsync();
}