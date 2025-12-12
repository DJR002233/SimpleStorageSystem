using System.Net.Http.Json;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;
using SimpleStorageSystem.Daemon.Services.Auth.TokenStore;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Mapper;

namespace SimpleStorageSystem.Daemon.Services.Auth;

public class AuthService
{
    private readonly IHttpClientFactory _httpFactory;

    private readonly ITokenStore _tokenStore;
    private readonly ICredentialStore _credentialStore;

    public AuthService(
        IHttpClientFactory httpFactory,
        ITokenStore tokenStore, ICredentialStore credentialsStore
    )
    {
        _httpFactory = httpFactory;
        _tokenStore = tokenStore;
        _credentialStore = credentialsStore;
    }

    public async Task<ApiResponse> LoginAsync(string email, string password)
    {
        try
        {
            var data = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var httpClient = _httpFactory.CreateClient("BasicClient");
            var httpResponse = await httpClient.PostAsJsonAsync("auth/login", data);

            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Session>>();

            if (apiResponse!.StatusMessage == ApiStatus.Success && apiResponse.Data is not null)
            {
                await _credentialStore.StoreAsync(apiResponse.Data.RefreshToken!);
                _tokenStore.SetAccessToken(apiResponse.Data.AccessToken!);
            }

            return apiResponse;
        }
        catch (HttpRequestException ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }
        catch (Exception ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }

    }

    public async Task<ApiResponse> CreateAccountAsync(string username, string email, string password)
    {
        try
        {
            var data = new CreateAccountRequest
            {
                Username = username,
                Email = email,
                Password = password,
            };

            var httpClient = _httpFactory.CreateClient("BasicClient");
            var httpResponse = await httpClient.PostAsJsonAsync("auth/sign_up", data);

            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>();

            return apiResponse!;
        }
        catch (HttpRequestException ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }
        catch (Exception ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }

    }

    public async Task<ApiResponse> LogoutAsync()
    {
        try
        {
            var httpClient = _httpFactory.CreateClient("LogoutClient");
            var apiResponse = await httpClient.GetFromJsonAsync<ApiResponse>("auth/logout");
            if (apiResponse!.StatusMessage == ApiStatus.Success)
            {
                await _credentialStore.DeleteAsync();
                _tokenStore.ClearAccessToken();
            }

            return apiResponse!;
        }
        catch (HttpRequestException ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }
        catch (Exception ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }

    }

    public async Task<ApiResponse> InitializeSessionAsync()
    {
        try
        {
            ApiResponse apiResponse = await _tokenStore.RefreshAccessTokenAsync();
            return apiResponse;
        }
        catch (HttpRequestException ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }
        catch (Exception ex)
        {
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }

    }

    public bool HasSession()
    {
        return _tokenStore.HasToken();
    }

    public bool HasActiveSession()
    {
        return _tokenStore.HasActiveToken();
    }

}
