using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Mapper;

namespace SimpleStorageSystem.Daemon.Services.Main;

public class StorageDriveService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StorageDriveService(IHttpClientFactory httpFactory, IServiceScopeFactory serviceScopeFactory)
    {
        _httpFactory = httpFactory;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async ValueTask<List<StorageDriveIpcDTO>> GetStorageDrives()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

        var storageDrives = dbContext.Drives.AsNoTracking().Where(d => d.DeletionTime != null).AsAsyncEnumerable();
        var listOfDrives = new List<StorageDriveIpcDTO>();

        await foreach (var drive in storageDrives)
        {
            listOfDrives.Add(ModelMapper.Map<StorageDriveIpcDTO>(drive));
        }

        return listOfDrives;
    }

    public async ValueTask<ApiResponse> CreateStorageDrive(string name)
    {
        StorageDriveRequest data = new StorageDriveRequest
        {
            Name = name
        };

        var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        var httpResponse = await httpClient.PostAsJsonAsync("storage_drive/create_drive", data);
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<StorageDriveApiDTO>>();

        if (apiResponse!.StatusCode == HttpStatusCode.OK)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

            dbContext.Drives.Add(
                new StorageDrive
                {
                    StorageDriveId = apiResponse.Data!.Id ?? throw new Exception("Missing Id from ApiResponse"),
                    Name = name
                }
            );
            
            await dbContext.SaveChangesAsync();
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
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<StorageDriveApiDTO>>();

        if (apiResponse!.StatusCode == HttpStatusCode.OK)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
            var drive = await dbContext.Drives.SingleOrDefaultAsync(d => d.StorageDriveId == id);

            if (drive is null)
            {
                dbContext.Drives.Add(
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

            await dbContext.SaveChangesAsync();
        }

        return apiResponse;
    }

    public async ValueTask<ApiResponse> DeleteStorageDrive(long id)
    {
        var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        var apiResponse = await httpClient.DeleteFromJsonAsync<ApiResponse>($"storage_drive/{id}/delete_drive");

        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

        if (apiResponse!.StatusCode == HttpStatusCode.OK)
            await dbContext.Drives.Where(d => d.StorageDriveId == id).ExecuteDeleteAsync();

        return apiResponse!;
    }

}
