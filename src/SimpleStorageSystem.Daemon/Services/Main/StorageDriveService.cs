// using System.Net;
// using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using SimpleStorageSystem.Daemon.Data;
using SimpleStorageSystem.Daemon.Services.Auth.CredentialStore;

// using SimpleStorageSystem.Daemon.Models.Tables;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;

// using SimpleStorageSystem.Shared.Enums;
// using SimpleStorageSystem.Shared.Models;
// using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Services.Mapper;

namespace SimpleStorageSystem.Daemon.Services.Main;

public class StorageDriveService
{
    // private readonly IHttpClientFactory _httpFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ICredentialStore _credentialStore;

    public StorageDriveService(
        // IHttpClientFactory httpFactory, 
        IServiceScopeFactory serviceScopeFactory, 
        ICredentialStore credentialStore)
    {
        // _httpFactory = httpFactory;
        _serviceScopeFactory = serviceScopeFactory;
        _credentialStore = credentialStore;
    }

    public async ValueTask<List<StorageDriveIpcDTO>> GetStorageDrivesAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

        var storageDrives = dbContext.StorageDrives.AsNoTracking().AsAsyncEnumerable();
        var listOfDrives = new List<StorageDriveIpcDTO>();

        await foreach (var drive in storageDrives)
        {
            listOfDrives.Add(ModelMapper.Map<StorageDriveIpcDTO>(drive));
        }

        return listOfDrives;
    }

    // public async ValueTask<ApiResponse> AddStorageDrive(string name)
    // {
    //     StorageDriveRequest data = new StorageDriveRequest
    //     {
    //         Name = name
    //     };

    //     var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
    //     var httpResponse = await httpClient.PostAsJsonAsync("storage_drive/create_drive", data);
    //     var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<StorageDriveApiDTO>>();

    //     if (apiResponse!.StatusCode == HttpStatusCode.OK)
    //     {
    //         using var scope = _serviceScopeFactory.CreateScope();
    //         var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

    //         dbContext.StorageDrives.Add(
    //             new StorageDrive
    //             {
    //                 StorageDriveId = apiResponse.Data!.Id ?? throw new Exception("Missing Id from ApiResponse"),
    //                 Name = name
    //             }
    //         );
            
    //         await dbContext.SaveChangesAsync();
    //     }

    //     return apiResponse;
    // }

    public async ValueTask RenameStorageDriveAsync(Guid id, string name)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

        var record = await dbContext.StorageDrives.SingleOrDefaultAsync( sd => sd.StorageDriveId == id) ??
        throw new Exception("Storage drive not found");

        record.Name = name;

        int rowsAffected = await dbContext.SaveChangesAsync();
        if (rowsAffected <= 0) throw new Exception("Renaming Failed");
        // StorageDriveRequest data = new StorageDriveRequest
        // {
        //     Name = name
        // };

        // var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        // var httpResponse = await httpClient.PutAsJsonAsync($"storage_drives/{id}/rename_drive", data);
        // var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<StorageDriveApiDTO>>();

        // if (apiResponse!.StatusCode == HttpStatusCode.OK)
        // {
        //     using var scope = _serviceScopeFactory.CreateScope();
        //     var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
        //     var drive = await dbContext.StorageDrives.SingleOrDefaultAsync(d => d.StorageDriveId == id);

        //     if (drive is null)
        //     {
        //         dbContext.StorageDrives.Add(
        //             new StorageDrive
        //             {
        //                 StorageDriveId = apiResponse.Data!.Id ?? throw new Exception("Missing Id from ApiResponse"),
        //                 Name = name
        //             }
        //         );
        //     }
        //     else
        //     {
        //         drive.StorageDriveId = apiResponse.Data!.Id ?? throw new Exception("Missing Id from ApiResponse");
        //         drive.Name = name;
        //     }

        //     await dbContext.SaveChangesAsync();
        // }

        // return apiResponse;
    }

    public async ValueTask DisconnectStorageDriveAsync(Guid id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

        var record = await dbContext.StorageDrives.SingleOrDefaultAsync( sd => sd.StorageDriveId == id) ??
        throw new Exception("Storage drive not found");

        dbContext.StorageDrives.Remove(record);

        int rowsAffected = await dbContext.SaveChangesAsync();
        if (rowsAffected <= 0) throw new Exception("Deleting Failed");
        
        await _credentialStore.DeleteAsync(id.ToString());
        // var httpClient = _httpFactory.CreateClient(HttpClientName.AuthenticatedClient.ToString());
        // var apiResponse = await httpClient.DeleteFromJsonAsync<ApiResponse>($"storage_drive/{id}/delete_drive");

        // using var scope = _serviceScopeFactory.CreateScope();
        // var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

        // if (apiResponse!.StatusCode == HttpStatusCode.OK)
        //     await dbContext.StorageDrives.Where(d => d.StorageDriveId == id).ExecuteDeleteAsync();

        // return apiResponse!;
    }

    public async ValueTask GetStorageDriveInformation(Guid id)
    {
        await Task.Delay(1000);
        throw new Exception("unimplemented");
        // return everything in StorageDriveIpcDTO and RootFolderIpcDTO
    }

    public async ValueTask MountStorageDrive(Guid id, MountOption mountOption)
    {
        await Task.Delay(1000);
        throw new Exception("unimplemented");
        // update MountOption to mountOption of StorageDrive
    }

    public async ValueTask UmountStorageDrive(Guid id)
    {
        await Task.Delay(1000);
        throw new Exception("unimplemented");
        // update MountOption to Inactive of StorageDrive
    }

}
