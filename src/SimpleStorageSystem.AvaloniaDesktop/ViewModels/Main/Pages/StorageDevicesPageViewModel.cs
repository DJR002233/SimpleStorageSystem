using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.Services.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public class StorageDrivesPageViewModel : ReactiveObject
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

    public StorageDrivesPageViewModel(
        LoadingOverlay loadingOverlay
    )
    {
        LoadingOverlay = loadingOverlay;
        Items = new ObservableCollection<Info>
        {
            new Info {Name = "Main Android", Icon = "Smartphone"},
            new Info {Name = "Shared", Icon = "Harddisk"},
            new Info {Name = "My Laptop", Icon = "Laptop"},
            new Info {Name = "i5-4460_xubuntu.minimal", Icon = "Linux"},
            new Info {Name = "i5-4460_win10", Icon = "Windows"},
        };
    }
    
}

public class Info
{
    public required string Name { get; set; }
    public required string Icon { get; set; }
}
