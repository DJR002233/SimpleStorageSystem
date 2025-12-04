using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Daemon.Services.Auth.TokenStore;

public interface ITokenStore
{
    Task<ApiResponse<string?>> GetTokenAsync();
    Task<ApiResponse<AccessToken>> GetAccessTokenAsync();
    void SetAccessToken(AccessToken accessToken);
    void ClearAccessToken();
    Task<ApiResponse> RefreshAccessTokenAsync();
    bool HasToken();
    bool HasActiveToken();

}
