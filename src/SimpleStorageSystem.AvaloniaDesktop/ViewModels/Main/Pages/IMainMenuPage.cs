using System;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

public interface IMainMenuPage
{
    string Name { get; }
    Type PageType { get; }
}
