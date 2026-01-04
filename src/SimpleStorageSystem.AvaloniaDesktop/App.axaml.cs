using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SimpleStorageSystem.AvaloniaDesktop.ServiceCollections;
using SimpleStorageSystem.AvaloniaDesktop.ViewModels;
using SimpleStorageSystem.AvaloniaDesktop.Views;
using Splat;

namespace SimpleStorageSystem.AvaloniaDesktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = default!;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Locator.CurrentMutable.RegisterViewsForViewModels(
            Assembly.GetExecutingAssembly()
        );
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // CultureInfo.CurrentCulture = new CultureInfo("en-PH");
            // CultureInfo.CurrentUICulture = new CultureInfo("en-PH");

            var services = new ServiceCollection();
            
            services.InitializeBaseServices();
            services.InitializeViewModelServices();
            services.InitializeMainMenuPagesServices();

            services.AddTransient(sp => new MainWindow
            {
                DataContext = sp.GetRequiredService<MainWindowViewModel>()
            });

            Services = services.BuildServiceProvider();

            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}