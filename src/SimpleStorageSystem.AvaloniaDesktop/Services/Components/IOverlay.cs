using ReactiveUI;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Components;

public interface IOverlay
{
    ReactiveObject? OverlayViewHost { get; set; }
}
