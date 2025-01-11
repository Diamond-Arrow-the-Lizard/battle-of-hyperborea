namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;

public static class GameBoardRenderer
{
    public static void DrawBoard(GameBoard board)
    {
        // Рендерим верхнюю границу поля
        for (int x = 0; x < board.Width; x++)
        {
            Console.Write(x == 0 ? "╔═══" : "╦═══");
        }
        Console.WriteLine("╗");

        for (int y = 0; y < board.Height; y++)
        {
            // Рендерим содержимое строк
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

                Console.Write(x == 0 ? $"║ {symbol} " : $"║ {symbol} ");
            }
            Console.WriteLine("║");

            // Рендерим соединительную или нижнюю границу
            if (y < board.Height - 1) // Если не последняя строка
            {
                for (int x = 0; x < board.Width; x++)
                {
                    Console.Write(x == 0 ? "╠═══" : "╬═══");
                }
                Console.WriteLine("╣");
            }
            else // Для последней строки
            {
                for (int x = 0; x < board.Width; x++)
                {
                    Console.Write(x == 0 ? "╚═══" : "╩═══");
                }
                Console.WriteLine("╝");
            }
        }
    }
}
