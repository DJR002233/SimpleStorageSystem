using System;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Components;

public class LoadingOverlay : ReactiveObject
{
    private readonly IOverlay _overlay;
    #region Property
    [Reactive] public string? Message { get; set; }
    #endregion Property

    public LoadingOverlay(IOverlay overlay)
    {
        _overlay = overlay;
    }
    
    private void Show(string message)
    {
        Message = message;
        _overlay.OverlayViewHost = new LoadingOverlayViewModel(message);
    }
    
    private void Hide()
    {
        _overlay.OverlayViewHost = null;
        Message = null;
    }

    public async ValueTask FromAsync(Func<ValueTask> method, string message = "Loading...")
    {
        Show(message);
        await method();
        Hide();
    }

    public async ValueTask FromAsync(Func<Task> method, string message = "Loading...")
    {
        Show(message);
        await method();
        Hide();
    }

    public async ValueTask<T> FromAsync<T>(Func<ValueTask<T>> method, string message = "Loading...")
    {
        Show(message);
        var data = await method();
        Hide();

        return data;
    }

    public async ValueTask<T> FromAsync<T>(Func<Task<T>> method, string message = "Loading...")
    {
        Show(message);
        var data = await method();
        Hide();

        return data;
    }

}
