using System.Net;
using System.Net.Http.Json;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;
using SimpleStorageSystem.Daemon.Services.Auth.TokenStore;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Helper;

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

    public async ValueTask<ApiResponse> LoginAsync(string email, string password)
    {
        var data = new LoginRequest
        {
            Email = email,
            Password = password
        };

        try
        {
            var httpClient = _httpFactory.CreateClient(HttpClientName.BasicClient.ToString());
            var httpResponse = await httpClient.PostAsJsonAsync("auth/login", data);

            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Session>>();

            if (apiResponse!.StatusCode == HttpStatusCode.OK && apiResponse.Data is not null)
            {
                await _credentialStore.StoreAsync(apiResponse.Data.RefreshToken!);
                _tokenStore.SetAccessToken(apiResponse.Data.AccessToken!);
            }

            // note to self: Convert Errors property to string and put in Message property
            return apiResponse;
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResponse { StatusCode = ex.StatusCode, Message = "Incorrect Credentials!" };

            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = ex.StatusCode, Message = ex.Message };
        }
        catch (Exception ex)
        {
            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = HttpStatusCode.BadGateway, Message = ex.Message };
        }

    }

    public async ValueTask<ApiResponse> CreateAccountAsync(string username, string email, string password)
    {
        var data = new CreateAccountRequest
        {
            Username = username,
            Email = email,
            Password = password,
        };

        try
        {
            var httpClient = _httpFactory.CreateClient(HttpClientName.BasicClient.ToString());
            var httpResponse = await httpClient.PostAsJsonAsync("auth/sign_up", data);

            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>();

            return apiResponse!;
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResponse { StatusCode = ex.StatusCode, Message = "Creation of account is not available!" };

            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = ex.StatusCode, Message = ex.Message };
        }
        catch (Exception ex)
        {
            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = HttpStatusCode.BadGateway, Message = ex.Message };
        }

    }

    public async ValueTask<ApiResponse> LogoutAsync()
    {
        try
        {
            var httpClient = _httpFactory.CreateClient(HttpClientName.LogoutClient.ToString());
            var apiResponse = await httpClient.PostAsync("auth/logout", null);
            
            if (apiResponse.IsSuccessStatusCode)
            {
                await _credentialStore.DeleteAsync();
                _tokenStore.ClearAccessToken();
            }

            return new ApiResponse { StatusCode = HttpStatusCode.NoContent };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResponse { StatusCode = ex.StatusCode, Message = "Please login again" };

            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = ex.StatusCode, Message = ex.Message };
        }
        catch (Exception ex)
        {
            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = HttpStatusCode.BadGateway, Message = ex.Message };
        }
    }

    public async ValueTask<ApiResponse> InitializeSessionAsync()
    {
        try
        {
            ApiResponse apiResponse = await _tokenStore.RefreshAccessTokenAsync();
            return apiResponse;
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return new ApiResponse { StatusCode = ex.StatusCode, Message = "Token Expired!" };

            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = ex.StatusCode, Message = ex.Message };
        }
        catch (Exception ex)
        {
            return new ApiResponse { Title = ex.GetType().ToString(), StatusCode = HttpStatusCode.BadGateway, Message = ex.Message };
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
