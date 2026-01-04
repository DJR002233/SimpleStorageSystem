using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using ReactiveUI.Avalonia;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Main.Pages;

public partial class StorageDrivesPageView : ReactiveUserControl<StorageDrivesPageViewModel>
{
    public StorageDrivesPageView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.AddStorageDriveCommand, v => v.AddStorageDriveButton).DisposeWith(disposables);
            // this.OneWayBind(ViewModel, vm => vm.Items, v => v.StorageDeviceItemsRepeater.ItemsSource).DisposeWith(disposables);
        });
    }

}
