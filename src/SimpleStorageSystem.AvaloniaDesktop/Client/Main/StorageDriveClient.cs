using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.Services;
using SimpleStorageSystem.Shared.DTOs;
using SimpleStorageSystem.Shared.Enums;
using SimpleStorageSystem.Shared.Models;
using SimpleStorageSystem.Shared.Requests;

namespace SimpleStorageSystem.AvaloniaDesktop.Client.Main;

public class StorageDriveClient
{
    public async ValueTask<IpcResponse<List<StorageDriveIpcDTO>>> RequestGetStorageDriveList()
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.GetStorageDriveList);

        try
        {
            PipeClient pipeClient = new PipeClient();

            await pipeClient.PostMessageAsync(ipcRequest);
            
            return pipeClient.GetResponse<List<StorageDriveIpcDTO>>();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateFromIpcRequest<List<StorageDriveIpcDTO>>(ipcRequest, IpcStatus.Error, ex.Message);
        }
    }

    public async ValueTask<IpcResponse<StorageDriveIpcDTO>> RequestAddStorageDriveList(string name)
    {
        var data = new StorageDriveRequest { Name = name };
        var ipcRequest = IpcRequest.Create(IpcCommand.AddStorageDrive, data);

        try
        {
            PipeClient pipeClient = new PipeClient();

            await pipeClient.PostMessageAsync(ipcRequest);
            
            return pipeClient.GetResponse<StorageDriveIpcDTO>();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateFromIpcRequest<StorageDriveIpcDTO>(ipcRequest, IpcStatus.Error, ex.Message);
        }
    }

    public async ValueTask<IpcResponse> RequestUpdateStorageDriveList(StorageDriveIpcDTO data)
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.UpdateAccount, data);

        try
        {
            PipeClient pipeClient = new PipeClient();

            await pipeClient.PostMessageAsync(ipcRequest);
            
            return pipeClient.GetResponse();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateFromIpcRequest(ipcRequest, IpcStatus.Error, ex.Message);
        }
    }

    public async ValueTask<IpcResponse> RequestDeleteStorageDriveList(StorageDriveIpcDTO data)
    {
        var ipcRequest = IpcRequest.Create(IpcCommand.DeleteStorageDrive, data);

        try
        {
            PipeClient pipeClient = new PipeClient();

            await pipeClient.PostMessageAsync(ipcRequest);
            
            return pipeClient.GetResponse();
        }
        catch(Exception ex)
        {
            return IpcResponse.CreateFromIpcRequest(ipcRequest, IpcStatus.Error, ex.Message);
        }
    }

}
