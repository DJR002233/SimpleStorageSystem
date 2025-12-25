using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;
using SimpleStorageSystem.Shared.Results;

namespace SimpleStorageSystem.AvaloniaDesktop.Client.Main;

public class StorageDriveClient
{
    public async ValueTask<IpcResponse<List<StorageDriveResult>>> RequestGetStorageDriveList()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.GetStorageDriveList);

        try
        {
            PipeClient pipeClient = new PipeClient();

            await pipeClient.PostMessageAsync(ipcRequest);
            
            return pipeClient.GetResponse<List<StorageDriveResult>>();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateErrorResponseFromIpcRequest<List<StorageDriveResult>>(ipcRequest, ex.Message);
        }
    }

}
