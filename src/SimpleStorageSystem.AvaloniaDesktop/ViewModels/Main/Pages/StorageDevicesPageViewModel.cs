using System.Reactive.Disposables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;
using SimpleStorageSystem.Shared.Results;
using System.Reactive.Linq;
using System;
using SimpleStorageSystem.Shared.Models;
using System.Collections.Generic;
using SimpleStorageSystem.AvaloniaDesktop.Client.Main;
using SimpleStorageSystem.Shared.Enums;
using DynamicData;
using SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

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
    [Reactive] public ObservableCollection<StorageDriveResult> Drives { get; set; }
    #endregion Properties

    public StorageDrivesPageViewModel(
        LoadingOverlay loadingOverlay,
        StorageDriveClient storageDriveClient
    )
    {
        LoadingOverlay = loadingOverlay;

        _storageDriveClient = storageDriveClient;

        Drives = new ObservableCollection<StorageDriveResult>();

        this.WhenActivated(disposables =>
        {
            Observable.FromAsync(GetStorageDrives).Subscribe().DisposeWith(disposables);
        });
    }

    public async Task GetStorageDrives()
    {
        IpcResponse<List<StorageDriveResult>> res = await _storageDriveClient.RequestGetStorageDriveList();

        if (res.Status == IpcStatus.Ok)
        {
            Drives.Add(res.Payload!);
            return;
        }

        await DialogBox.Show(res.Status.ToString(), res.Message);
    }
    
}
