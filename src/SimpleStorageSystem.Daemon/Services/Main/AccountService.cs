using System.Net.Http.Json;
using AutoMapper;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Helper;

namespace SimpleStorageSystem.Daemon.Services.Main;

public class AccountService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IMapper _mapper;

    public AccountService(
        IHttpClientFactory httpFactory,
        IMapper mapper
    )
    {
        _httpFactory = httpFactory;
        _mapper = mapper;
    }

    public async Task<ApiResponse> UpdateAccountInformationAsync(string? username, string? email, string? password)
    {
        try
        {
            var data = new UpdateAccountRequest
            {
                Username = username,
                Email = email,
                Password = password
            };
            var httpClient = _httpFactory.CreateClient("AuthenticatedClient");
            var response = await httpClient.PutAsJsonAsync("api/account/update", data);

            var res = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return res ?? CreateApiResponse.Failed("Unknown Error");
        }
        catch (HttpRequestException ex)
        {
            ApiResponse apiResponse = _mapper.Map<ApiResponse>(ex);
            return apiResponse;
        }
        catch (Exception ex)
        {
            ApiResponse apiResponse = _mapper.Map<ApiResponse>(ex);
            return apiResponse;
        }

    }

}
