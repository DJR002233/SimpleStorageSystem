using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.Daemon.Services.Main;

public class StorageDriveService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly SqLiteDbContext _dbContext;

    public StorageDriveService(IHttpClientFactory httpFactory, SqLiteDbContext dbContext)
    {
        _httpFactory = httpFactory;
        _dbContext = dbContext;
    }

    public async ValueTask<ApiResponse<List<StorageDriveDTO>>> GetStorageDrives()
    {
        var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        var apiResponse = await httpClient.GetFromJsonAsync<ApiResponse<List<StorageDriveDTO>>>("storage_drive/get_drives");

        return apiResponse!;
    }

    public async ValueTask<ApiResponse> CreateStorageDrive(string name)
    {
        StorageDriveRequest data = new StorageDriveRequest
        {
            Name = name
        };

        var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        var httpResponse = await httpClient.PostAsJsonAsync("storage_drive/create_drive", data);
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<StorageDriveDTO>>();

        if (apiResponse!.StatusMessage == ApiStatus.Success)
        {
            _dbContext.Drives.Add(
                new StorageDrive
                {
                    StorageDriveId = apiResponse.Data!.Id ?? throw new Exception("Missing Id from ApiResponse"),
                    Name = name
                }
            );
            await _dbContext.SaveChangesAsync();
        }

        return apiResponse;
    }

    public async ValueTask<ApiResponse> RenameStorageDrive(long id, string name)
    {
        StorageDriveRequest data = new StorageDriveRequest
        {
            Name = name
        };

        var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        var httpResponse = await httpClient.PutAsJsonAsync($"storage_drives/{id}/rename_drive", data);
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<StorageDriveDTO>>();

        if (apiResponse!.StatusMessage == ApiStatus.Success)
        {
            var drive = await _dbContext.Drives.SingleOrDefaultAsync(d => d.StorageDriveId == id);

            if(drive is null)
            {
                _dbContext.Drives.Add(
                    new StorageDrive
                    {
                        StorageDriveId = apiResponse.Data!.Id ?? throw new Exception("Missing Id from ApiResponse"),
                        Name = name
                    }
                );
            }
            else
            {
                drive.StorageDriveId = apiResponse.Data!.Id ?? throw new Exception("Missing Id from ApiResponse");
                drive.Name = name;
            }
            
            await _dbContext.SaveChangesAsync();
        }

        return apiResponse;
    }

    public async ValueTask<ApiResponse> DeleteStorageDrive(long id)
    {
        var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        var apiResponse = await httpClient.DeleteFromJsonAsync<ApiResponse>($"storage_drive/{id}/delete_drive");

        if(apiResponse!.StatusMessage == ApiStatus.Success)
            await _dbContext.Drives.Where(d => d.StorageDriveId == id).ExecuteDeleteAsync();

        return apiResponse!;
    }
    
}
