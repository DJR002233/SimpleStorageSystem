using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Models.Auth;
using SimpleStorageSystem.AvaloniaDesktop.Services.CredentialStore;
using SimpleStorageSystem.AvaloniaDesktop.Services.TokenStore;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Auth;

public class SessionManager : ISessionManager
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    private readonly ITokenStore _tokenStore;
    private readonly ICredentialStore _credentialStore;

    public async Task<bool> SetSessionAsync(Session session)
    {
        return await _tokenStore.SetSessionAsync(session);
    }

    public async Task<Response> InitializeSessionAsync()
    {
        string? refreshToken = await _credentialStore.GetAsync();
        
        if (String.IsNullOrWhiteSpace(refreshToken))
            return new Response { StatusMessage = StatusMessage.Unauthenticated };

        _tokenStore.RefreshToken = refreshToken;

        Response res = await _tokenStore.GetAccessTokenAsync();

        return res;
    }

    public async Task<Response> TerminateSessionAsync()
    {
        try
        {
            var res = await _httpClient.GetFromJsonAsync<Response>("accounts/logout");

            if (res!.StatusMessage == StatusMessage.Success || res.StatusMessage == StatusMessage.Failed)
            {
                await _tokenStore.SetSessionAsync(new Session {});
                await _credentialStore.DeleteAsync();
            }

            return res;
        } catch (HttpRequestException ex)
        {
            return _mapper.Map<Response>(ex);
        } catch (Exception ex)
        {
            return _mapper.Map<Response>(ex);;
        }
    }

    public SessionManager(
        HttpClient httpClient, IMapper mapper,
        ITokenStore tokenStore, ICredentialStore credentialsStore
    )
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _tokenStore = tokenStore;
        _credentialStore = credentialsStore;
    }

}
