using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

public class LoadingOverlayViewModel : ReactiveObject
{
    #region Property
    [Reactive] public string? Message { get; set; }
    #endregion Property

    public LoadingOverlayViewModel(string? message)
    {
        Message = message;
    }

}
