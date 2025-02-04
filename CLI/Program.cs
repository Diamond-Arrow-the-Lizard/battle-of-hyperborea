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
        await Task.Delay(100);
        GameBoard gameBoard = new GameBoard(5, 5);
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
