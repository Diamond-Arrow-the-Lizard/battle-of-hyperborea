namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;

public static class GameBoardRenderer
{
    public static void DrawBoard(GameBoard board)
    {
        // Верхняя граница
        Console.Write("╔");
        for (int x = 0; x < board.Width; x++)
        {
            Console.Write("═");
            if (x < board.Width - 1)
                Console.Write("╦");
        }
        Console.WriteLine("╗");

        // Рисуем строки игрового поля
        for (int y = 0; y < board.Height; y++)
        {
            Console.Write("║");
            for (int x = 0; x < board.Width; x++)
            {
                var cellContent = board[x, y].Content;

                char symbol = cellContent switch
                {
                    null => ' ', // Пустая клетка
                    Obstacle obstacle => obstacle.Icon,
                    IUnit unit => unit.Icon,
                    _ => '?'
                };
                Console.Write($"{symbol}║");
            }
            Console.WriteLine();

            // Горизонтальная разделительная линия (кроме последней строки)
            if (y < board.Height - 1)
            {
                Console.Write("╠");
                for (int x = 0; x < board.Width; x++)
                {
                    Console.Write("═");
                    if (x < board.Width - 1)
                        Console.Write("╬");
                }
                Console.WriteLine("╣");
            }
        }

        // Нижняя граница
        Console.Write("╚");
        for (int x = 0; x < board.Width; x++)
        {
            Console.Write("═");
            if (x < board.Width - 1)
                Console.Write("╩");
        }
        Console.WriteLine("╝");
    }
}
