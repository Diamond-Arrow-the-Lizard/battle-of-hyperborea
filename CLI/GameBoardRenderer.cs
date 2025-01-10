namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;

public static class GameBoardRenderer
{
    public static void DrawBoard(GameBoard board)
    {
        for (int x = 0; x < board.Width; x++)
        {
            Console.WriteLine();
            /* //TODO need to refactor
            for (int y = 0; y < board.Width; y++)
            {
                switch(board[x, y].CellType)
                {
                    case CellType.Empty:
                    Console.Write("O ");
                    break;

                    case CellType.Obstacle:
                    Console.Write("B ");
                    break;

                    case CellType.Unit:
                    Console.Write($"T ");
                    break;
                }

            }
            */
        }
    }
}