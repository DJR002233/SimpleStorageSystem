using System.Threading.Tasks;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Components;

public class DialogBox
{
    private readonly IOverlay _overlay;

    public DialogBox(IOverlay overlay)
    {
        _overlay = overlay;
    }

    public async ValueTask<bool> ShowConfirmation(string message, string? title, bool OkOnly = true)
    {
        var dialog = new ConfirmationDialogBoxViewModel(message, title, OkOnly);
        _overlay.OverlayViewHost = dialog;

        bool result = await dialog.Result;
        _overlay.OverlayViewHost = null;

        return result;
    }
    public async ValueTask<bool> ShowConfirmation(string message, bool OkOnly = true)
    {
        return await ShowConfirmation(message, null, OkOnly);
    }

    public async ValueTask<string?> ShowMessageBox(string message, string? title = null)
    {
        var dialog = new MessageDialogBoxViewModel(message, title);
        _overlay.OverlayViewHost = dialog;

        string? result = await dialog.Result;
        _overlay.OverlayViewHost = null;

        return result;
    }
    
    // public async ValueTask ShowOkOnly(string message, string title, SystemDecorations decorations = SystemDecorations.Full)
    // {
    //     var dialog = new DialogBoxView(title, message, decorations);

    //     var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

    //     await dialog.ShowDialog(lifetime!.MainWindow!);
    // }

    // public async ValueTask<string?> ShowTextInput(string message, string title, SystemDecorations decorations = SystemDecorations.Full)
    // {
    //     var dialog = new DialogBoxView(title, message, DialogBoxMode.InputText, decorations);

    //     var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

    //     return await dialog.ShowDialog<string?>(lifetime!.MainWindow!);
    // }

    // public async ValueTask ShowOkOnly(string message)
    // {
    //     await ShowOkOnly(message, "");
    // }

    // public async ValueTask ShowTextInput(string message)
    // {
    //     await ShowOkOnly(message, "");
    // }

}
