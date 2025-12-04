using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using SimpleStorageSystem.AvaloniaDesktop.Views.Components;

namespace SimpleStorageSystem.AvaloniaDesktop.Services.Helper;

public static class DialogBox
{
    public static async Task Show(string? title, string? message)
    {
        var dialog = new DialogBoxView(title, message);

        var lifetime = Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        await dialog.ShowDialog(lifetime!.MainWindow!);
    }
}
