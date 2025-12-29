using System.Reactive.Disposables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using System.Reactive.Linq;
using System;
using SimpleStorageSystem.Shared.Models;
using System.Collections.Generic;
using SimpleStorageSystem.AvaloniaDesktop.Client.Main;
using SimpleStorageSystem.Shared.Enums;
using DynamicData;
using SimpleStorageSystem.Shared.DTOs;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class StorageDrivesPageViewModel : ReactiveObject, IActivatableViewModel
{
    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    public ViewModelActivator Activator { get; } = new();
    #endregion Services

    #region Clients
    private readonly StorageDriveClient _storageDriveClient;
    #endregion Clients

    #region VMs
    #endregion VMs
    
    #region Commands
    #endregion Commands

    #region Properties
    [Reactive] public ObservableCollection<StorageDriveIpcDTO> Drives { get; set; }
    #endregion Properties

    public StorageDrivesPageViewModel(
        LoadingOverlay loadingOverlay,
        StorageDriveClient storageDriveClient
    )
    {
        LoadingOverlay = loadingOverlay;

        _storageDriveClient = storageDriveClient;

        Drives = new ObservableCollection<StorageDriveIpcDTO>();

        this.WhenActivated(disposables =>
        {
            Observable.FromAsync(GetStorageDrives).Subscribe().DisposeWith(disposables);
        });
    }

    public async Task GetStorageDrives()
    {
        IpcResponse<List<StorageDriveIpcDTO>> ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestGetStorageDriveList(), "Obtaining list of drives...");

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            Drives.Add(ipcResponse.Payload!);
            return;
        }

        await DialogBox.ShowOk(ipcResponse.Status.ToString()!, ipcResponse.Message!);
    }

    public async Task AddStorageDrive()
    {
        IpcResponse<StorageDriveIpcDTO> ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestAddStorageDriveList(""), "Creating storage drive...");

        if (ipcResponse.Status == IpcStatus.Ok && ipcResponse.Payload is not null)
        {
            Drives.Add(ipcResponse.Payload);
            return;
        }

        await DialogBox.ShowOk(ipcResponse.Status.ToString()!, ipcResponse.Message!);
    }

    // public async Task UpdateStorageDrive()
    // {
    //     IpcResponse<StorageDriveIpcDTO> ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestAddStorageDriveList(""), "Creating storage drive...");

    //     if (ipcResponse.Status == IpcStatus.Ok && ipcResponse.Payload is not null)
    //     {
    //         Drives.Add(ipcResponse.Payload);
    //         return;
    //     }

    //     await DialogBox.Show(ipcResponse.Status.ToString(), ipcResponse.Message);
    // }

    // public async Task DeleteStorageDrive()
    // {
    //     IpcResponse<StorageDriveIpcDTO> ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestAddStorageDriveList(""), "Creating storage drive...");

    //     if (ipcResponse.Status == IpcStatus.Ok && ipcResponse.Payload is not null)
    //     {
    //         Drives.Add(ipcResponse.Payload);
    //         return;
    //     }

    //     await DialogBox.Show(ipcResponse.Status.ToString(), ipcResponse.Message);
    // }

}
