using System.Net;
using System.Net.Http.Json;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Mapper;

namespace SimpleStorageSystem.Daemon.Services.Main;

public class AccountService
{
    private readonly IHttpClientFactory _httpFactory;

    public AccountService(
        IHttpClientFactory httpFactory
    )
    {
        _httpFactory = httpFactory;
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
            var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
            var response = await httpClient.PutAsJsonAsync("account/update", data);

            var res = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return res ?? new ApiResponse { StatusCode = HttpStatusCode.BadRequest, Message = "Unknown Error" };
        }
        catch (HttpRequestException ex)
        {
            // Note to self: don't use model mapper
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }
        catch (Exception ex)
        {
            // Note to self: don't use model mapper
            ApiResponse apiResponse = ModelMapper.Map<ApiResponse>(ex);
            return apiResponse;
        }

    }

}
