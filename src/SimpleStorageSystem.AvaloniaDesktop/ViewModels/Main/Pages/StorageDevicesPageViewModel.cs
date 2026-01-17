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
using System.Reactive.Disposables.Fluent;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class StorageDrivesPageViewModel : ReactiveObject, IActivatableViewModel, IMainMenuPage
{
    #region Services
    private readonly LoadingOverlay _loadingOverlay;
    private readonly DialogBox _dialogBox;
    public ViewModelActivator Activator { get; } = new();
    #endregion Services

    #region Clients
    private readonly StorageDriveClient _storageDriveClient;
    #endregion Clients

    #region Commands
    public ReactiveCommand<Unit, Unit> AddStorageDriveCommand { get; }
    public ReactiveCommand<Guid, Unit> RenameStorageDriveCommand { get; }
    public ReactiveCommand<Guid, Unit> DeleteStorageDriveCommand { get; }
    public ReactiveCommand<Guid, Unit> OpenStorageDriveInformationCommand { get; }
    #endregion Commands

    #region Properties
    public string PageName => "Storage Drive";
    public Type PageType => GetType();
    [Reactive] public ObservableCollection<StorageDriveIpcDTO> Drives { get; set; }
    #endregion Properties

    public StorageDrivesPageViewModel(LoadingOverlay loadingOverlay, DialogBox dialogBox, StorageDriveClient storageDriveClient)
    {
        _loadingOverlay = loadingOverlay;
        _dialogBox = dialogBox;

        _storageDriveClient = storageDriveClient;

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
        IpcResponse<List<StorageDriveIpcDTO>> ipcResponse = await _loadingOverlay.FromAsync(() => _storageDriveClient.RequestGetStorageDriveList(), "Obtaining list of drives...");

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

        await _dialogBox.ShowConfirmation(ipcResponse.Message!, ipcResponse.Status.ToString());
    }

    public async Task AddStorageDrive()
    {
        string? name = await _dialogBox.ShowMessageBox("Enter a name:", "Add Storage Drive");
        if (String.IsNullOrWhiteSpace(name)) return;

        IpcResponse<StorageDriveIpcDTO> ipcResponse = await _loadingOverlay.FromAsync(() => _storageDriveClient.RequestAddStorageDrive(name), "Creating storage drive...");
        if (ipcResponse.Status == IpcStatus.Ok && ipcResponse.Payload is not null)
        {
            Drives.Add(ipcResponse.Payload);
            return;
        }

        await _dialogBox.ShowConfirmation(ipcResponse.Message!, ipcResponse.Status.ToString()!);
    }

    public async Task RenameStorageDrive(Guid id)
    {
        string? name = await _dialogBox.ShowMessageBox("Enter a name:", "Rename Storage Drive");
        if (String.IsNullOrWhiteSpace(name)) return;

        IpcResponse ipcResponse = await _loadingOverlay.FromAsync(() => _storageDriveClient.RequestRenameStorageDrive(id, name), "Renaming storage drive...");

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            var drive = Drives.SingleOrDefault(d => d.StorageDriveId == id) ?? throw new Exception("Drive not found while renaming!");
            drive.Name = "";
            return;
        }

        await _dialogBox.ShowConfirmation(ipcResponse.Message!, ipcResponse.Status.ToString()!);
    }

    public async Task DeleteStorageDrive(Guid id)
    {
        bool IsTrue = await _dialogBox.ShowConfirmation("Are you sure you want to disconnect this drive?\n\nThis action will not delete files.", "Disconnect Storage Drive", OkOnly: false);
        if (!IsTrue) return;

        IpcResponse ipcResponse = await _loadingOverlay.FromAsync(() => _storageDriveClient.RequestDisconnectStorageDrive(id), "Disconnecting drive...");

        if (ipcResponse.Status == IpcStatus.Ok)
        {
            var drive = Drives.Where(d => d.StorageDriveId == id) ?? throw new Exception("Drive not found while deleting!");
            Drives.Remove(drive);
            return;
        }

        await _dialogBox.ShowConfirmation(ipcResponse.Message!, ipcResponse.Status.ToString()!);
    }

    public void OpenStorageDriveInformation(Guid id)
    {
        // _mainMenuVM().StorageDriveInformationView(id);
    }

}
