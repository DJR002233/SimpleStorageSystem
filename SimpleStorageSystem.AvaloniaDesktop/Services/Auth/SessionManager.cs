using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.CredentialsStore;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public class SessionManager : ISessionManager
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ICredentialsStore _credentialsStore;

    private readonly Session _session;

    public SessionManager(HttpClient httpClient, IMapper mapper, ICredentialsStore credentialsStore, Session session)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _credentialsStore = credentialsStore;

        _session = session;
    }

    public async Task<Response<string?>> GetAccessTokenAsync()
    {
        if (_session.Expiration > DateTime.UtcNow && !String.IsNullOrWhiteSpace(_session.AccessToken))
            return new Response<string?>
            {
                StatusMessage = StatusMessage.Success,
                Data = _session.AccessToken
            };
        if (String.IsNullOrWhiteSpace(_session.RefreshToken))
            return new Response<string?> { StatusMessage = StatusMessage.Unauthenticated };
        
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Response<Session>>("accounts/renew_access_token");

            if (response!.StatusMessage == StatusMessage.Success && response.Data is not null)
            {
                await SetSessionAsync(response.Data);
                Response<string?> res = _mapper.Map<Response<string?>>(response);
                res.Data = response.Data.AccessToken;
                return res;
            }
            
            return _mapper.Map<Response<string?>>(response);
        } catch (HttpRequestException ex)
        {
            return _mapper.Map<Response<string?>>(ex);
        } catch (Exception ex)
        {
            return _mapper.Map<Response<string?>>(ex);
        }

    }

    public string? GetRefreshToken()
    {
        return _session.RefreshToken;
    }

    public async Task<bool> SetSessionAsync(Session session)
    {
        if (ModelChecker.AnyPropertyIsNullorWhiteSpace(session))
            return false;
        
        _session.AccessToken = session.AccessToken;
        _session.Expiration = session.Expiration;
        _session.RefreshToken = session.RefreshToken;

        await _credentialsStore.StoreAsync(session.RefreshToken!);

        return true;
    }
    
    public async Task<Response> InitializeSessionAsync()
    {
        string? refreshToken = await _credentialsStore.GetAsync();

        if (String.IsNullOrWhiteSpace(refreshToken))
            return new Response { StatusMessage = StatusMessage.Unauthenticated };

        _session.RefreshToken = refreshToken;

        Response res = await GetAccessTokenAsync();

        return res;
    }

    public async Task<Response> TerminateSessionAsync()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _session.AccessToken);
        await DialogBox.Show("Session Manager", "TerminateSession() ran");
        return new Response {};
    }

    // public void LogSession()
    // {
    //     Console.WriteLine($"AccessToken: {_session.AccessToken}");
    //     Console.WriteLine($"Expiration: {_session.Expiration}");
    //     Console.WriteLine($"RefreshToken: {_session.RefreshToken}");
    // }

}