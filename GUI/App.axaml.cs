using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using BoH.GUI.ViewModels;
using BoH.GUI.Views;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using BoH.Interfaces;
using BoH.Models;
using BoH.Services;
using BoH.GameLogic;

namespace BoH.GUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var collection = new ServiceCollection();
            collection = ProvideServices();

            var services = collection.BuildServiceProvider();

            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>(),
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


    private static ServiceCollection ProvideServices()
    {
        var collection = new ServiceCollection();

        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<GameBoardViewModel>();


        collection.AddSingleton<IGameBoard, GameBoard>();
        collection.AddSingleton<IGameBoardService, GameBoardService>();
        collection.AddSingleton<ITurnManager, TurnManager>();


        return collection;
    }

}