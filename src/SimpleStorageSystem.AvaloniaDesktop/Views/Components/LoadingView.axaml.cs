using Avalonia;
using Avalonia.ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Views.Components;

public partial class LoadingOverlay : ReactiveUserControl<object>
{
    public static readonly StyledProperty<bool> VisibleProperty =
        AvaloniaProperty.Register<LoadingOverlay, bool>(nameof(Visible));
    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<LoadingOverlay, string>(nameof(Message), "Loading...");

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

    public LoadingOverlay()
    {
        InitializeComponent();
    }
}
