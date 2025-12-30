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
using System.Reactive;
using System.Linq;
using Avalonia.Controls;

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
    private readonly Func<MainMenuViewModel> _mainMenuVM;
    #endregion VMs
    
    #region Commands
    public ReactiveCommand<Unit, Unit> AddStorageDriveCommand { get; }
    public ReactiveCommand<long, Unit> RenameStorageDriveCommand { get; }
    public ReactiveCommand<long, Unit> DeleteStorageDriveCommand { get; }
    public ReactiveCommand<long, Unit> OpenStorageDriveInformationCommand { get; }
    #endregion Commands

    #region Properties
    [Reactive] public ObservableCollection<StorageDriveIpcDTO> Drives { get; set; }
    #endregion Properties

    public StorageDrivesPageViewModel(
        LoadingOverlay loadingOverlay,
        StorageDriveClient storageDriveClient,
        Func<MainMenuViewModel> mainMenuVM
    )
    {
        LoadingOverlay = loadingOverlay;

        _storageDriveClient = storageDriveClient;

        _mainMenuVM = mainMenuVM;

        Drives = new ObservableCollection<StorageDriveIpcDTO>();

        AddStorageDriveCommand = ReactiveCommand.CreateFromTask(AddStorageDrive);
        RenameStorageDriveCommand = ReactiveCommand.CreateFromTask<long>(RenameStorageDrive);
        DeleteStorageDriveCommand = ReactiveCommand.CreateFromTask<long>(DeleteStorageDrive);
        OpenStorageDriveInformationCommand = ReactiveCommand.Create<long>(OpenStorageDriveInformation);

        this.WhenActivated(disposables =>
        {
            Observable.FromAsync(GetStorageDrives).Subscribe().DisposeWith(disposables);
        });
    }

    public async Task GetStorageDrives()
    {
        IpcResponse<List<StorageDriveIpcDTO>> ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestGetStorageDriveList(), "Obtaining list of drives...");

        // test content
        ipcResponse.Payload = new List<StorageDriveIpcDTO>
        {
            new StorageDriveIpcDTO { Id = 0, Name = "PC" },
            new StorageDriveIpcDTO { Id = 1, Name = "Smartphone" },
            new StorageDriveIpcDTO { Id = 2, Name = "Ubuntu" },
            new StorageDriveIpcDTO { Id = 3, Name = "Windows" },
            new StorageDriveIpcDTO { Id = 4, Name = "Android" },
            new StorageDriveIpcDTO { Id = 5, Name = "Laptop" },
        };

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            Drives.Clear();
            Drives.Add(ipcResponse.Payload!);
            return;
        }

        await DialogBox.ShowOk(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public async Task AddStorageDrive()
    {
        string? name = await DialogBox.ShowTextInput("Add Storage Drive","Enter a name:");
        if (String.IsNullOrWhiteSpace(name)) return;

        IpcResponse<StorageDriveIpcDTO> ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestAddStorageDriveList(name), "Creating storage drive...");

        if (ipcResponse.Status == IpcStatus.Ok && ipcResponse.Payload is not null)
        {
            Drives.Add(ipcResponse.Payload);
            return;
        }

        await DialogBox.ShowOk(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public async Task RenameStorageDrive(long id)
    {
        string? name = await DialogBox.ShowTextInput("Rename Storage Drive","Enter a name:");
        if (String.IsNullOrWhiteSpace(name)) return;

        var data = new StorageDriveIpcDTO { Id = id, Name = name };

        IpcResponse ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestRenameStorageDriveList(data), "Renaming storage drive...");

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            var drive = Drives.SingleOrDefault( d => d.Id == id) ?? throw new Exception("Drive not found while renaming!");
            drive.Name = "";
            return;
        }

        await DialogBox.ShowOk(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public async Task DeleteStorageDrive(long id)
    {
        IpcResponse ipcResponse = await LoadingOverlay.FromAsync( () => _storageDriveClient.RequestDeleteStorageDriveList(id), "Deleting storage drive...");

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            var drive = Drives.Where( d => d.Id == id) ?? throw new Exception("Drive not found while deleting!");
            Drives.Remove(drive);
            return;
        }

        await DialogBox.ShowOk(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public void OpenStorageDriveInformation(long id)
    {
        _mainMenuVM().StorageDriveInformationView(id);
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
