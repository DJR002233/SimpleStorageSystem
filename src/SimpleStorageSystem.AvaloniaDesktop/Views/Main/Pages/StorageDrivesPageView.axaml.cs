using Avalonia.ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Main.Pages;

public partial class StorageDrivesPageView : ReactiveUserControl<StorageDrivesPageViewModel>
{
    public StorageDrivesPageView()
    {
        InitializeComponent();
        // this.WhenActivated(disposables =>
        // {
        //     // this.OneWayBind(ViewModel, vm => vm.Items, v => v.StorageDeviceItemsRepeater.ItemsSource).DisposeWith(disposables);
        // });

    }

}
