using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using SimpleStorageSystem.AvaloniaDesktop.Models;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ISessionManager _sessionManager;

    public AuthService(HttpClient httpClient, IMapper mapper, ISessionManager sessionManager)
    {
        _httpClient = httpClient;
        _mapper = mapper;
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

            var res = await response.Content.ReadFromJsonAsync<Response<Session>>();

            if (res!.StatusMessage == StatusMessage.Success && res.Data is not null)
                await _sessionManager.SetSessionAsync(res.Data);

            return res;
        } catch (HttpRequestException ex)
        {
            return _mapper.Map<Response>(ex);
        } catch (Exception ex)
        {
            return _mapper.Map<Response>(ex);;
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

            var res = await response.Content.ReadFromJsonAsync<Response>();

            return res!;
        } catch (HttpRequestException ex)
        {
            return _mapper.Map<Response>(ex);
        } catch (Exception ex)
        {
            return _mapper.Map<Response>(ex);
        }

    }

    public async Task<Response> ResumeSessionAsync()
    {
        try
        {
            Response res = await _sessionManager.InitializeSessionAsync();

            return res;
        } catch (HttpRequestException ex)
        {
            return _mapper.Map<Response>(ex);
        } catch (Exception ex)
        {
            return _mapper.Map<Response>(ex);
        }

    }
    
}
