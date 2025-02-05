namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.GameLogic;

public class Program
{

    /// <summary>
    /// Точка входа в приложение. Отвечает за запуск игры и обработку игровых раундов.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    public static async Task Main(string[] args)
    {
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

        foreach(var i in units)
        Console.WriteLine(i.ToString());

        players[0] = new Player("Rus");
        players[1] = new Player("Lizard");

        GameBoard gameBoard = (GameBoard)gameBoardService.GenerateGameBoard(8, 8, units, players);
        ActionHandler actionHandler = new(gameBoard);
        ScannerHandler scannerHandler = new(gameBoard);
        List<ICell> scannedCells = new();

        ConsoleGameBoardRenderer renderer = new ConsoleGameBoardRenderer();
        actionHandler.OnUpdatingGameBoard += renderer.Render;
        scannerHandler.OnGameBoardScanned += renderer.ScanRender;


        BaseUnit unit = new("Guy");
        actionHandler.HandleSkip(unit);
        scannedCells = scannerHandler.HandleScan(gameBoard[1, 1], 2);
    }

}
