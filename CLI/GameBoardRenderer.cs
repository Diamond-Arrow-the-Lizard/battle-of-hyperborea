namespace BoH.CLI;

using BoH.GameLogic;
using BoH.Interfaces;
using BoH.Models;

public static class GameBoardRenderer
{
    public static void DrawBoard(IGameBoard board, ICell? cellScan = null, int? scanRange = null)
    {
        IEnumerable<ICell>? scannedCells = null;

        if (cellScan != null && scanRange != null)
        {
            Scanner scanner = new Scanner((int)scanRange);
            scannedCells = scanner.Scan(cellScan, board);
        }

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

                // Обновляем содержимое клеток, если мы сканируем
                if (scannedCells != null)
                {
                    if (scannedCells.Any(c => c.Position == board[x, y].Position))
                    {
                        var scannedContent = board[x, y].Content;
                        symbol = scannedContent switch
                        {
                            null => '#', // Пустая клетка
                            Obstacle obstacle => obstacle.Icon,
                            IUnit => '!',
                            _ => '?'
                        };

                    }
                }

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
