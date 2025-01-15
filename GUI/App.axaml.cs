using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using BoH.GUI.ViewModels;
using BoH.GUI.Views;
using BoH.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using BoH.Interfaces;

namespace BoH.GUI;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    public T GetService<T>() where T : notnull
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("ServiceProvider is not initialized.");
        }

        return _serviceProvider.GetRequiredService<T>();
    }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider() ?? throw new NullReferenceException("Service Provider is null");

            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            // Устанавливаем DataContext для MainWindow
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IGameBoardService, GameBoardService>();
        services.AddSingleton<GameBoardViewModel>();
        services.AddSingleton<MainWindowViewModel>();
        //services.AddSingleton<LoadButtonViewModel>();
        //services.AddSingleton<SaveButtonViewModel>();
        services.AddSingleton<GameBoardView>();
        services.AddSingleton<MainWindow>();
        //services.AddSingleton<LoadButtonView>();
        //services.AddSingleton<SaveButtonView>();
    }
}