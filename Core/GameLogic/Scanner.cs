namespace BoH.Models;

using System.Collections.Generic;
using BoH.Interfaces;

/// <summary>
/// Реализация сканера, который определяет клетки в радиусе вокруг заданной клетки.
/// </summary>
public class Scanner : IScanner
{
    /// <summary>
    /// Радиус сканирования.
    /// </summary>
    public int Range { get; }

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
    public IEnumerable<ICell> Scan(ICell scanningCell, IGameBoard gameBoard)
    {
        if (scanningCell == null)
        {
            throw new ArgumentNullException(nameof(scanningCell), "Сканируемая клетка не может быть null.");
        }

        if (gameBoard == null)
        {
            throw new ArgumentNullException(nameof(gameBoard), "Игровое поле не может быть null.");
        }

        var (x, y) = scanningCell.Position;
        var scannedCells = new List<ICell>();

        for (int i = x - Range; i <= x + Range; i++)
        {
            for (int j = y - Range; j <= y + Range; j++)
            {
                if (i >= 0 && i < gameBoard.Width && j >= 0 && j < gameBoard.Height && gameBoard[i, j].CellType != CellType.Obstacle)
                {
                    scannedCells.Add(gameBoard[i, j]);
                }
            }
        }

        return scannedCells;
    }
}
