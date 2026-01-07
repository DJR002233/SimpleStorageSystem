using System;
using Microsoft.Extensions.DependencyInjection;
using SimpleStorageSystem.AvaloniaDesktop.Services.PageFactory;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main.Pages;

namespace SimpleStorageSystem.AvaloniaDesktop.ViewModels.Main;

public class MainMenuPageFactory : IPageFactory<IMainMenuPage>
{
    private readonly IServiceProvider _sp;

    public MainMenuPageFactory(IServiceProvider sp)
    {
        _sp = sp;
    }

    public IMainMenuPage Create(Type pageType)
        => (IMainMenuPage)_sp.GetRequiredService(pageType);
}
