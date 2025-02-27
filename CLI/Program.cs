namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.GameLogic;
using System.Diagnostics;

public class Program
{

    /// <summary>
    /// Точка входа в приложение. Отвечает за запуск игры и обработку игровых раундов.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    public static async Task Main(string[] args)
    {
        // Сетап игры
        // ------------------------------------------------------------------------------------------------------
        GameBoardService gameBoardService = new GameBoardService();
        GameController gameController = new GameController(gameBoardService);
        await Task.Delay(100);
        Player[] players = new Player[2];
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

        ConsoleAbilityNotifications abilityNotifications = new ConsoleAbilityNotifications();
        ConsoleUnitNotifications unitNotifications = new ConsoleUnitNotifications();

        foreach (var i in units)
        {
            i.OnAttack += unitNotifications.Notify_UnitAttacked;
            foreach(var j in i.Abilities)
            {
                j.OnAbilityUsed += abilityNotifications.Notify_AbilityUsed;
                j.OnCooldown += abilityNotifications.Notify_AbilityOnCooldown;
            }
            i.OnTakingDamage += unitNotifications.Notify_UnitRecievedDamage;
        }

        players[0] = new Player("Rus");
        players[1] = new Player("Lizard");

        GameBoard gameBoard = (GameBoard)gameBoardService.GenerateGameBoard(8, 8, units, players);
        ActionHandler actionHandler = new(gameBoard);
        ScannerHandler scannerHandler = new(gameBoard);
        List<ICell> scannedCells = new();

        ConsoleGameBoardRenderer renderer = new ConsoleGameBoardRenderer();
        actionHandler.OnUpdatingGameBoard += renderer.Render;
        scannerHandler.OnGameBoardScanned += renderer.ScanRender;


        TurnManager turnManager = new TurnManager(gameBoard, players, actionHandler, scannerHandler);

        turnManager.OnUnitSelected += unitNotifications.Notify_UnitSelected;


        // ------------------------------------------------------------------------------------------------------

        turnManager.StartNewRound(players[0]);
        renderer.Render(gameBoard);
        turnManager.SelectUnit(gameBoard[1, 0]);
        scannedCells = turnManager.ProcessScanner();
        turnManager.ProcessPlayerAction(scannedCells, gameBoard[3, 3]);
        turnManager.SelectUnit(gameBoard[3, 3]);
        scannedCells = turnManager.ProcessScanner();
        turnManager.ProcessPlayerAction();

        turnManager.EndTurn();

        turnManager.StartNewRound(players[1]);
        turnManager.SelectUnit(gameBoard[6, 7]);
        scannedCells = turnManager.ProcessScanner();
        turnManager.ProcessPlayerAction(scannedCells, gameBoard[5, 5]);
        turnManager.SelectUnit(gameBoard[5, 5]);
        IUnit unit = gameBoard[5, 5].Content as IUnit ?? throw new ArgumentNullException("");
        scannedCells = turnManager.ProcessScanner(unit.Abilities[1]);
        turnManager.ProcessPlayerAction(scannedCells, null, unit.Abilities[1]);
        turnManager.SelectUnit(gameBoard[5, 5]);
        turnManager.ProcessPlayerAction(scannedCells, gameBoard[4, 4]);
        turnManager.SelectUnit(gameBoard[4, 4]);
        scannedCells = turnManager.ProcessScanner(unit.Abilities[0]);
        unit = gameBoard[4, 4].Content as IUnit ?? throw new ArgumentNullException("");
        turnManager.ProcessPlayerAction(scannedCells, gameBoard[3, 3], unit.Abilities[0]);

        // ... Проходит ход...

    }

}
