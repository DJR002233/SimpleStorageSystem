using Avalonia;
using ReactiveUI.Avalonia;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Components;

public partial class LoadingOverlayView : ReactiveUserControl<object>
{
    public LoadingOverlayView()
    {
        InitializeComponent();
    }
    
    public static readonly StyledProperty<bool> VisibleProperty =
        AvaloniaProperty.Register<LoadingOverlayView, bool>(nameof(Visible));
    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<LoadingOverlayView, string>(nameof(Message), "Loading...");

    public bool Visible
    {
        get => GetValue(VisibleProperty);
        set => SetValue(VisibleProperty, value);
    }

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

}
