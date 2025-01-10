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
            for (int y = 0; y < board.Width; y++)
            {
                if(board.Cells[x, y].Content == null)
                {
                    Console.Write("O ");
                }

            }
        }
    }
}