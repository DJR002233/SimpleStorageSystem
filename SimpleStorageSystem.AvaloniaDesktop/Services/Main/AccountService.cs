using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using SimpleStorageSystem.AvaloniaDesktop.Models;
using SimpleStorageSystem.AvaloniaDesktop.Models.Account;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Main;

public class AccountService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public AccountService(HttpClient httpClient, IMapper mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<Response> UpdateAccountInformation(string? username, string? email, string? password)
    {
        try
        {
            AccountInformationDTO data = new AccountInformationDTO
            {
                Username = username,
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("accounts/update_information", data);

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
    
}
