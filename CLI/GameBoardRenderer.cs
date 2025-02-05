namespace BoH.CLI;

using BoH.GameLogic;
using BoH.Interfaces;
using BoH.Models;

public class ConsoleGameBoardRenderer : IGameBoardRenderer
{
    public void Render(IGameBoard gameBoard)
    {
        // TODO
        Console.WriteLine("Cool game board");
        for (int x = 0; x < gameBoard.Width; x++)
        {
            for (int y = 0; y < gameBoard.Width; y++)
            {
                if (gameBoard[x, y] is Cell cell)
                    Console.Write($"{cell.Icon} ");
            }
            Console.WriteLine();
        }
    }
    public void ScanRender(IGameBoard gameBoard, List<ICell> scannedCells)
    {
        // TODO
        Console.WriteLine($"Cool game board (scanned {scannedCells.Count} cells)");
        for (int x = 0; x < gameBoard.Width; x++)
        {
            for (int y = 0; y < gameBoard.Width; y++)
            {
                if (gameBoard[x, y] is Cell cell)
                {
                    if (scannedCells.Contains(cell))
                    {
                        if (cell.Content is IUnit unit)
                            Console.Write("! ");
                        else
                            Console.Write("# ");
                    }
                    else
                    {
                        Console.Write($"{cell.Icon} ");

                    }
                }
            }
            Console.WriteLine();
        }
    }
}
