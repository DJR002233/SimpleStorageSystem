using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ISessionManager _sessionManager;

    public AuthService(HttpClient httpClient, ISessionManager sessionManager)
    {
        _httpClient = httpClient;
        _sessionManager = sessionManager;
    }

    public async Task<Response> LoginAsync(string email, string password)
    {
        try
        {
            var loginData = new
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("accounts/login", loginData);
            
            var content = await response.Content.ReadFromJsonAsync<ApiResponse<Session>>();
            var res = content!.MapToResponse();

            if (res.StatusMessage == StatusMessage.Success && res.Data is not null)
                await _sessionManager.SetSessionAsync(res.Data);

            return res;
        } catch (HttpRequestException ex)
        {
            return ResponseMapper.MapHttpRequestException(ex);
        } catch (Exception ex)
        {
            return ResponseMapper.MapException(ex);
        }

    }

    public async Task<Response> CreateAccountAsync(string username, string email, string password)
    {
        try
        {
            var AccountData = new
            {
                Email = email,
                Password = password,
                Username = username
            };
            var response = await _httpClient.PostAsJsonAsync("accounts/sign_up", AccountData);

            var content = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return content!.MapToResponse();
        } catch (HttpRequestException ex)
        {
            return ResponseMapper.MapHttpRequestException(ex);
        } catch (Exception ex)
        {
            return ResponseMapper.MapException(ex);
        }

    }

    public async Task<Response> InitializeSession()
    {
        try
        {
            await _sessionManager.InitializeSessionAsync();
            Response res = new Response{};
            return res;
        }catch (Exception ex){
            return ResponseMapper.MapException(ex);
        }
    }
}
