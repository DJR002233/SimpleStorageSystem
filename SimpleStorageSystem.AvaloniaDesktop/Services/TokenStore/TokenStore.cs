using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Models.Auth;
using SimpleStorageSystem.AvaloniaDesktop.Services.CredentialStore;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.TokenStore;

public class TokenStore : ITokenStore
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ICredentialStore _credentialStore;

    private readonly Session _session;

    public string? RefreshToken { get => _session.RefreshToken; set => _session.RefreshToken = value; }

    public async Task<bool> SetSessionAsync(Session session)
    {
        if (ModelChecker.AnyPropertyIsNullorWhiteSpace(session))
            return false;

        _session.AccessToken = session.AccessToken;
        _session.Expiration = session.Expiration;
        _session.RefreshToken = session.RefreshToken;

        await _credentialStore.StoreAsync(session.RefreshToken!);

        return true;
    }

    public void ClearSession()
    {
        _session.AccessToken = null;
        _session.Expiration = null;
        _session.RefreshToken = null;
    }
    
    public async Task<Response<string?>> GetAccessTokenAsync()
    {
        if (String.IsNullOrWhiteSpace(_session.RefreshToken))
            return new Response<string?> { StatusMessage = StatusMessage.Unauthenticated };
        if (!String.IsNullOrWhiteSpace(_session.AccessToken))
            return new Response<string?>
            {
                StatusMessage = StatusMessage.Success,
                Data = _session.AccessToken
            };

        var response = await _httpClient.GetFromJsonAsync<Response<Session>>("accounts/renew_access_token");
        Response<string?> res = _mapper.Map<Response<string?>>(response);

        if (response!.StatusMessage == StatusMessage.Success && response.Data is not null)
        {
            await SetSessionAsync(response.Data);
            res.Data = response.Data.AccessToken;
            return res;
        }

        return res;
    }

    public TokenStore(
        HttpClient httpClient, IMapper mapper, ICredentialStore credentialStore,
        Session session
    )
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _credentialStore = credentialStore;

        _session = session;
    }

}
