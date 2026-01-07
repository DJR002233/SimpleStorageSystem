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
using System.Reactive.Disposables.Fluent;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class StorageDrivesPageViewModel : ReactiveObject, IActivatableViewModel, IMainMenuPage
{
    #region Services
    // public LoadingOverlay LoadingOverlay { get; }
    private readonly DialogBox _dialogBox;
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
    public ReactiveCommand<Guid, Unit> RenameStorageDriveCommand { get; }
    public ReactiveCommand<Guid, Unit> DeleteStorageDriveCommand { get; }
    public ReactiveCommand<Guid, Unit> OpenStorageDriveInformationCommand { get; }
    #endregion Commands

    #region Properties
    public string Name => "Storage Drive";
    public Type PageType => GetType();
    [Reactive] public ObservableCollection<StorageDriveIpcDTO> Drives { get; set; }
    #endregion Properties

    public StorageDrivesPageViewModel(
        DialogBox dialogBox, //LoadingOverlay loadingOverlay,
        StorageDriveClient storageDriveClient,
        Func<MainMenuViewModel> mainMenuVM
    )
    {
        // LoadingOverlay = loadingOverlay;
        _dialogBox = dialogBox;

        _storageDriveClient = storageDriveClient;

        _mainMenuVM = mainMenuVM;

        Drives = new ObservableCollection<StorageDriveIpcDTO>();

        AddStorageDriveCommand = ReactiveCommand.CreateFromTask(AddStorageDrive);
        RenameStorageDriveCommand = ReactiveCommand.CreateFromTask<Guid>(RenameStorageDrive);
        DeleteStorageDriveCommand = ReactiveCommand.CreateFromTask<Guid>(DeleteStorageDrive);
        OpenStorageDriveInformationCommand = ReactiveCommand.Create<Guid>(OpenStorageDriveInformation);

        this.WhenActivated(disposables =>
        {
            Observable.FromAsync(GetStorageDrives).Subscribe().DisposeWith(disposables);
        });
    }

    public async Task GetStorageDrives()
    {
        IpcResponse<List<StorageDriveIpcDTO>> ipcResponse = await _mainMenuVM().LoadingOverlay.FromAsync(() => _storageDriveClient.RequestGetStorageDriveList(), "Obtaining list of drives...");

        // test content
        ipcResponse.Status = IpcStatus.Ok;
        ipcResponse.Payload = new List<StorageDriveIpcDTO>
            {
                new StorageDriveIpcDTO { StorageDriveId = Guid.NewGuid(), Name = "PC" },
                new StorageDriveIpcDTO { StorageDriveId = Guid.NewGuid(), Name = "Smartphone" },
                new StorageDriveIpcDTO { StorageDriveId = Guid.NewGuid(), Name = "Ubuntu" },
                new StorageDriveIpcDTO { StorageDriveId = Guid.NewGuid(), Name = "Windows" },
                new StorageDriveIpcDTO { StorageDriveId = Guid.NewGuid(), Name = "Android" },
                new StorageDriveIpcDTO { StorageDriveId = Guid.NewGuid(), Name = "Laptop" },
            };

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            Drives.Clear();
            Drives.Add(ipcResponse.Payload!);
            return;
        }

        await _dialogBox.ShowOkOnly(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public async Task AddStorageDrive()
    {
        string? name = await _dialogBox.ShowTextInput("Add Storage Drive", "Enter a name:");
        if (String.IsNullOrWhiteSpace(name)) return;

        IpcResponse<StorageDriveIpcDTO> ipcResponse = await _mainMenuVM().LoadingOverlay.FromAsync(() => _storageDriveClient.RequestAddStorageDrive(name), "Creating storage drive...");
        if (ipcResponse.Status == IpcStatus.Ok && ipcResponse.Payload is not null)
        {
            Drives.Add(ipcResponse.Payload);
            return;
        }

        await _dialogBox.ShowOkOnly(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public async Task RenameStorageDrive(Guid id)
    {
        string? name = await _dialogBox.ShowTextInput("Rename Storage Drive", "Enter a name:");
        if (String.IsNullOrWhiteSpace(name)) return;

        IpcResponse ipcResponse = await _mainMenuVM().LoadingOverlay.FromAsync(() => _storageDriveClient.RequestRenameStorageDrive(id, name), "Renaming storage drive...");

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            var drive = Drives.SingleOrDefault(d => d.StorageDriveId == id) ?? throw new Exception("Drive not found while renaming!");
            drive.Name = "";
            return;
        }

        await _dialogBox.ShowOkOnly(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public async Task DeleteStorageDrive(Guid id)
    {
        IpcResponse ipcResponse = await _mainMenuVM().LoadingOverlay.FromAsync(() => _storageDriveClient.RequestDisconnectStorageDrive(id), "Drive not found while deleting!");

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            var drive = Drives.Where(d => d.StorageDriveId == id) ?? throw new Exception("Drive not found while deleting!");
            Drives.Remove(drive);
            return;
        }

        await _dialogBox.ShowOkOnly(ipcResponse.Status.ToString()!, ipcResponse.Message!, SystemDecorations.None);
    }

    public void OpenStorageDriveInformation(Guid id)
    {
        _mainMenuVM().StorageDriveInformationView(id);
    }

}
