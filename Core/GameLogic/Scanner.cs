namespace BoH.GameLogic;

using System.Collections.Generic;
using BoH.Interfaces;
using BoH.Models;

/// <summary>
/// Реализация сканера, который определяет клетки в радиусе вокруг заданной клетки.
/// </summary>
public class Scanner : IScanner
{
    /// <summary>
    /// Радиус сканирования.
    /// </summary>
    public int Range { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр сканера с заданным радиусом.
    /// </summary>
    /// <param name="range">Радиус сканирования.</param>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если радиус меньше 1.</exception>
    public Scanner(int range)
    {
        if (range < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(range), "Радиус сканирования должен быть больше нуля.");
        }

        Range = range;
    }

    /// <summary>
    /// Сканирует клетки вокруг заданной клетки на игровом поле Клетки с препятствием игнорируются.
    /// </summary>
    /// <param name="scanningCell">Клетка, вокруг которой производится сканирование.</param>
    /// <param name="gameBoard">Игровое поле, на котором производится сканирование.</param>
    /// <returns>Коллекция клеток в пределах радиуса вокруг заданной клетки.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если одна из переданных ссылок равна null.
    /// </exception>
    /// <summary>
    /// Сканирует клетки вокруг заданной клетки на игровом поле, игнорируя клетки с препятствиями.
    /// </summary>
    /// <param name="scanningCell">Клетка, вокруг которой производится сканирование.</param>
    /// <param name="gameBoard">Игровое поле, на котором производится сканирование.</param>
    /// <returns>Лист клеток в пределах радиуса вокруг заданной клетки, игнорируя препятствия.</returns>
    public List<ICell> Scan(ICell scanningCell, IGameBoard gameBoard)
    {
        if (scanningCell == null)
        {
            throw new ArgumentNullException(nameof(scanningCell), "Сканируемая клетка не может быть null.");
        }

        if (gameBoard == null)
        {
            throw new ArgumentNullException(nameof(gameBoard), "Игровое поле не может быть null.");
        }

        var (x, y) = scanningCell.Position;  // Получаем позицию сканируемой клетки
        var scannedCells = new List<ICell>();  // Список для хранения результатов сканирования

        // Проходим по всем клеткам в радиусе
        for (int i = x - Range; i <= x + Range; i++)
        {
            for (int j = y - Range; j <= y + Range; j++)
            {
                // Проверяем, что клетки находятся в пределах поля
                if (i >= 0 && i < gameBoard.Width && j >= 0 && j < gameBoard.Height)
                {
                    var cell = gameBoard[i, j];  // Получаем клетку по координатам (i, j)

                    if (!(cell.Content is Obstacle))
                    {
                        scannedCells.Add(cell);  // Добавляем подходящую клетку в список
                    }
                }
            }
        }

        return scannedCells;
    }
}
