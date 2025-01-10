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
                var cellContent = board[x, y].Content;
                
                if (cellContent is null)
                {
                    Console.Write("O "); // Пустая клетка
                }
                else if (cellContent is Obstacle obstacle)
                {
                    Console.Write($"{obstacle.Icon} "); // Препятствие
                }
                else if (cellContent is IUnit unit)
                {
                    Console.Write($"{unit.Icon} "); // Юнит 
                }
                else
                {
                    Console.Write("? "); // Непредвиденный случай
                }
            }
        }
    }
}
