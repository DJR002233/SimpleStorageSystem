using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Daemon.Services.Auth.TokenStore;

public interface ITokenStore
{
    ValueTask<ApiResponse<string?>> GetTokenAsync();
    ValueTask<ApiResponse<AccessToken>> GetAccessTokenAsync();
    void SetAccessToken(AccessToken accessToken);
    void ClearAccessToken();
    ValueTask<ApiResponse> RefreshAccessTokenAsync();
    bool HasToken();
    bool HasActiveToken();

}
