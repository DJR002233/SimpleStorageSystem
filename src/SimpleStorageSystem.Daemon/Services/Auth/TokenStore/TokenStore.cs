using System.Net.Http.Json;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Services.Helper;
using SimpleStorageSystem.Shared.Services.Mapper;

namespace SimpleStorageSystem.Daemon.Services.Auth.TokenStore;

public class TokenStore : ITokenStore
{
    private readonly IHttpClientFactory _httpFactory;

    private readonly SemaphoreSlim _lock = new(1);

    private readonly ICredentialStore _credentialStore;
    private readonly AccessToken _accessToken;

    public TokenStore(
        IHttpClientFactory httpFactory,
        ICredentialStore credentialStore
    )
    {
        _httpFactory = httpFactory;

        _credentialStore = credentialStore;
        _accessToken = new AccessToken { };
    }

    public async ValueTask<ApiResponse<string?>> GetTokenAsync()
    {
        await _lock.WaitAsync();

        try
        {
            ApiResponse<AccessToken> accessToken = await GetAccessTokenAsync();
            ApiResponse<string?> res = ModelMapper.Map<ApiResponse<string?>>(accessToken);
            res.Data = accessToken.Data?.Token;
            return res;
        }
        finally
        {
            _lock.Release();
        }

    }

    public async ValueTask<ApiResponse<AccessToken>> GetAccessTokenAsync()
    {
        await _lock.WaitAsync();

        try
        {
            if (
            !String.IsNullOrWhiteSpace(_accessToken.Token) &&
            _accessToken.Expiration is not null &&
            _accessToken.Expiration >= DateTime.UtcNow.AddMinutes(3)
            )
                return CreateApiResponse.Success(_accessToken);

            ApiResponse apiResponse = await RefreshAccessTokenAsync();
            if (apiResponse.StatusMessage == ApiStatus.Success)
                return CreateApiResponse.Success(_accessToken);

            return ModelMapper.Map<ApiResponse<AccessToken>>(apiResponse);
        }
        finally
        {
            _lock.Release();
        }

    }

    public void SetAccessToken(AccessToken accessToken)
    {
        _lock.Wait();

        try
        {
            if (ModelChecker.AnyPropertyIsNullorWhiteSpace(accessToken))
                throw new Exception("Access token is invalid!");

            _accessToken.SetNew(accessToken);
        }
        finally
        {
            _lock.Release();
        }

    }

    public void ClearAccessToken()
    {
        _lock.Wait();

        try
        {
            _accessToken.Clear();
        }
        finally
        {
            _lock.Release();
        }

    }

    public async ValueTask<ApiResponse> RefreshAccessTokenAsync()
    {
        // await _lock.WaitAsync();

        // try
        // {
            if (String.IsNullOrWhiteSpace(await _credentialStore.GetAsync()))
                return CreateApiResponse.Unauthenticated();

            var client = _httpFactory.CreateClient(HttpClientName.TokenClient.ToString());
            var apiResponse = await client.GetFromJsonAsync<ApiResponse<Session>>("auth/refresh_session");

            if (apiResponse!.StatusMessage == ApiStatus.Success && apiResponse.Data is not null)
            {
                await _credentialStore.StoreAsync(apiResponse.Data.RefreshToken!);
                _accessToken.SetNew(apiResponse.Data.AccessToken!);
            }

            return apiResponse;
        // }
        // finally
        // {
        //     _lock.Release();
        // }

    }

    public bool HasToken()
    {
        _lock.Wait();

        try
        {
            if (!String.IsNullOrWhiteSpace(_accessToken.Token))
                return true;
            return false;
        }
        finally
        {
            _lock.Release();
        }


    }

    public bool HasActiveToken()
    {
        _lock.Wait();

        try
        {
            if (
                !String.IsNullOrWhiteSpace(_accessToken.Token) &&
                _accessToken.Expiration is not null &&
                _accessToken.Expiration > DateTime.UtcNow
            )
                return true;
            return false;
        }
        finally
        {
            _lock.Release();
        }

    }

}
