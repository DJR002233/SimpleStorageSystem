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
    
    public void Close()
    {
        IsVisible = false;
        Message = null;
    }

}
