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
            i.OnAttack += unitNotifications.Notify_UnitRecievedDamage;
            i.Abilities[0].OnAbilityUsed += abilityNotifications.Notify_AbilityUsed;
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

        // Ход игрока 1
        turnManager.StartNewRound(players[0]);
        renderer.Render(gameBoard);
        turnManager.SelectUnit(gameBoard[1, 0]);
        scannedCells = turnManager.ProcessScanner(ActionType.Move);
        turnManager.ProcessPlayerAction(ActionType.Move, scannedCells, gameBoard[1, 1]);
        turnManager.SelectUnit(gameBoard[1, 1]);
        scannedCells = turnManager.ProcessScanner(ActionType.Attack);
        turnManager.ProcessPlayerAction(ActionType.Skip);

        // ... Проходит ход...

        // Ход игрока 2
        turnManager.EndTurn();
        gameController.CheckVictoryCondition(players);
        turnManager.StartNewRound(players[1]);
        turnManager.SelectUnit(gameBoard[6, 7]);
        scannedCells = turnManager.ProcessScanner(ActionType.Move);
        turnManager.ProcessPlayerAction(ActionType.Move, scannedCells, gameBoard[4, 4]);

        turnManager.SelectUnit(gameBoard[4, 4]);
        LizardWarrior selectedUnit = gameBoard[4, 4].Content as LizardWarrior ?? throw new ArgumentNullException();
        selectedUnit.Abilities[0].OnCooldown += abilityNotifications.Notify_AbilityOnCooldown;
        
        scannedCells = turnManager.ProcessScanner(ActionType.Ability);
        turnManager.ProcessPlayerAction(ActionType.Ability, scannedCells, null, selectedUnit.Abilities[0]);

        Console.WriteLine(selectedUnit.CurrentTurnPhase);
        Console.WriteLine(selectedUnit.Abilities[0]);

        turnManager.SelectUnit(selectedUnit.OccupiedCell!);
        scannedCells = turnManager.ProcessScanner(ActionType.Move);
        Console.WriteLine(selectedUnit.CurrentTurnPhase);
        Console.WriteLine(selectedUnit.Abilities[0]);
        turnManager.ProcessPlayerAction(ActionType.Move, scannedCells, gameBoard[2, 1]);

        Console.WriteLine(selectedUnit.CurrentTurnPhase);
        Console.WriteLine(selectedUnit.Abilities[0]);
        
        turnManager.SelectUnit(gameBoard[2, 1]!);
        scannedCells = turnManager.ProcessScanner(ActionType.Attack);
        turnManager.ProcessPlayerAction(ActionType.Attack, scannedCells, gameBoard[1, 1]);

        // ... Проходит ход...

        // ...
    }

}
