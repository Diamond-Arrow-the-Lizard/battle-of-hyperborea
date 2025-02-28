using System.Collections.Generic;
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
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        int width = 8;
        int height = 8;
        IPlayer[] players = new IPlayer[2];
        players[0] = new Player("Rus");
        players[1] = new Player("Lizard");
        List<IUnit> units = new(){
            new RusArcher(),
            new RusWarrior(),
            new LizardArcher(),
            new LizardWarrior(),
            new RusArcher(),
            new RusWarrior(),
            new LizardArcher(),
            new LizardWarrior(),
            new RusArcher(),
            new RusWarrior(),
            new LizardArcher(),
            new LizardWarrior()
        };

        collection.AddSingleton<IPlayer[]>(players);
        collection.AddSingleton<IActionHandler, ActionHandler>();
        collection.AddSingleton<IScanner, Scanner>();
        collection.AddSingleton<IScannerHandler, ScannerHandler>();
        collection.AddSingleton<IGameController, GameController>();
        collection.AddSingleton<IGameBoardService, GameBoardService>();
        collection.AddSingleton<ITurnManager, TurnManager>();
        
        collection.AddSingleton<IGameBoard>(sp =>
        {
            var gameBoardService = sp.GetRequiredService<IGameBoardService>();
            return gameBoardService.GenerateGameBoard(width, height, units, players as IPlayer[]);
        });
        
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<GameBoardViewModel>();
        collection.AddSingleton<AbilitiesViewModel>();
        collection.AddSingleton<SelectedUnitDetailsViewModel>();

        return collection;
    }

}