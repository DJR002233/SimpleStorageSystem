using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class StorageDevicesPageViewModel : ReactiveObject
{
    #region Services
    public LoadingOverlay LoadingOverlay { get; }
    #endregion Services

    #region VMs
    #endregion VMs
    
    #region Commands
    #endregion Commands

    #region Properties
    [Reactive] public ObservableCollection<Info> Items { get; set; }
    #endregion Properties

    public StorageDevicesPageViewModel(
        LoadingOverlay loadingOverlay
    )
    {
        LoadingOverlay = loadingOverlay;
        Items = new ObservableCollection<Info>
        {
            new Info {Name = "lol", Value = 100},
            new Info {Name = "lmao", Value = 202},
            new Info {Name = "kek", Value = 364},
            new Info {Name = "lmfao", Value = 635},
            new Info {Name = "hehe", Value = 586},
        };
    }
    
}

public class Info
{
    public required string Name { get; set; }
    public int Value { get; set; }
}
