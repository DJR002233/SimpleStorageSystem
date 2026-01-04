using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SimpleStorageSystem.AvaloniaDesktop.Enums;
using SimpleStorageSystem.AvaloniaDesktop.Views.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Components;

public class DialogBox
{
    public async ValueTask ShowOk(string title, string message, SystemDecorations decorations = SystemDecorations.Full)
    {
        var dialog = new DialogBoxView(title, message, decorations);

        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        await dialog.ShowDialog(lifetime!.MainWindow!);
    }

    public async ValueTask<string?> ShowTextInput(string title, string message, SystemDecorations decorations = SystemDecorations.Full)
    {
        var dialog = new DialogBoxView(title, message, DialogBoxMode.InputText, decorations);

        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        return await dialog.ShowDialog<string?>(lifetime!.MainWindow!);
    }

    public async ValueTask<bool> ShowConfirmation(string title, string message, SystemDecorations decorations = SystemDecorations.Full)
    {
        var dialog = new DialogBoxView(title, message, DialogBoxMode.ConfirmCancel, decorations);

        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        return await dialog.ShowDialog<bool?>(lifetime!.MainWindow!) ?? false;
    }

}
