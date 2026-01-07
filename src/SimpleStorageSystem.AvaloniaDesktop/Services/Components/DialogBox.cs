using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SimpleStorageSystem.AvaloniaDesktop.Enums;
using SimpleStorageSystem.AvaloniaDesktop.Views.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Components;

public class DialogBox
{
    public async ValueTask ShowOkOnly(string message, string title, SystemDecorations decorations = SystemDecorations.Full)
    {
        var dialog = new DialogBoxView(title, message, decorations);

        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        await dialog.ShowDialog(lifetime!.MainWindow!);
    }

    public async ValueTask<string?> ShowTextInput(string message, string title, SystemDecorations decorations = SystemDecorations.Full)
    {
        var dialog = new DialogBoxView(title, message, DialogBoxMode.InputText, decorations);

        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        return await dialog.ShowDialog<string?>(lifetime!.MainWindow!);
    }

    public async ValueTask<bool> ShowConfirmation(string message, string title, SystemDecorations decorations = SystemDecorations.Full)
    {
        var dialog = new DialogBoxView(title, message, DialogBoxMode.ConfirmCancel, decorations);

        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        return await dialog.ShowDialog<bool?>(lifetime!.MainWindow!) ?? false;
    }

    public async ValueTask ShowOkOnly(string message)
    {
        await ShowOkOnly(message, "");
    }

    public async ValueTask ShowTextInput(string message)
    {
        await ShowOkOnly(message, "");
    }

    public async ValueTask ShowConfirmation(string message)
    {
        await ShowOkOnly(message, "");
    }

}
