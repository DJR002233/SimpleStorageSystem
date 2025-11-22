using System;
using System.Net.Http;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public class SessionManager : ISessionManager
{
    private readonly HttpClient _httpClient;
    private static readonly Session _session = new Session{};


    public SessionManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        await DialogBox.Show("","");
        return _session.AccessToken;
    }

    public string? GetRefreshToken()
    {
        return _session.RefreshToken;
    }

    public async Task<bool> SetSessionAsync(Session session)
    {
        await DialogBox.Show("Session Manager","SetSession() ran");
        return false;
    }
    
    public async Task<Response> InitializeSessionAsync()
    {
        await DialogBox.Show("Session Manager","InitializeSession() ran");
        return new Response {};
    }

    public async Task<Response> TerminateSessionAsync()
    {
        await DialogBox.Show("Session Manager","TerminateSession() ran");
        return new Response {};
    }
}