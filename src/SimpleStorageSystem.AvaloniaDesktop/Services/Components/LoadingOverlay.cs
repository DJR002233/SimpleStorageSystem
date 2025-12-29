using System;
using System.Threading.Tasks;
using ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Components;

public class LoadingOverlay : ReactiveObject
{
    #region Field
    private bool _isVisible;
    private string? _message;

    #endregion Field
    
    #region Property
    public bool IsVisible
    { 
        get => _isVisible; 
        set => this.RaiseAndSetIfChanged(ref _isVisible, value);
    }
    public string? Message 
    { 
        get => _message; 
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    #endregion Property

    public LoadingOverlay()
    {
        IsVisible = false;
    }
    
    public void Show(string message)
    {
        Message = message;
        IsVisible = true;
    }
    
    public void Hide()
    {
        IsVisible = false;
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
