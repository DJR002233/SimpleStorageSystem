using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Models.Account;
using SimpleStorageSystem.AvaloniaDesktop.Models.Auth;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    private readonly ISessionManager _sessionManager;

    public AuthService(
        HttpClient httpClient, IMapper mapper,
        ISessionManager sessionManager
    )
    {
        _httpClient = httpClient;
        _mapper = mapper;

        _sessionManager = sessionManager;
    }

    public async Task<Response> LoginAsync(string email, string password)
    {
        try
        {
            var data = new LoginCredentialsDTO
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("accounts/login", data);

            var res = await response.Content.ReadFromJsonAsync<Response<Session>>();

            if (res!.StatusMessage == StatusMessage.Success && res.Data is not null)
                await _sessionManager.SetSessionAsync(res.Data);

            return res;
        }
        catch(HttpRequestException ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "HttpRequestException";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }
        catch(Exception ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "Exception";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }

    }

    public async Task<Response> CreateAccountAsync(string username, string email, string password)
    {
        try
        {
            var data = new AccountInformationDTO
            {
                Username = username,
                Email = email,
                Password = password,
            };
            var response = await _httpClient.PostAsJsonAsync("accounts/sign_up", data);

            var res = await response.Content.ReadFromJsonAsync<Response>();

            return res!;
        }
        catch(HttpRequestException ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "HttpRequestException";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }
        catch(Exception ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "Exception";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }

    }

    public async Task<Response> LogoutAsync()
    {
        try
        {
            Response res = await _sessionManager.TerminateSessionAsync();
            return res;
        }
        catch(HttpRequestException ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "HttpRequestException";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }
        catch(Exception ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "Exception";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }

    }

    public async Task<Response> ResumeSessionAsync()
    {
        try
        {
            Response res = await _sessionManager.InitializeSessionAsync();

            return res;
        }
        catch(HttpRequestException ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "HttpRequestException";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }
        catch(Exception ex)
        {
            Response res = _mapper.Map<Response>(ex);
            res.Title = "Exception";
            res.StatusMessage = StatusMessage.Error;
            return res;
        }

    }

}
