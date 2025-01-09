namespace BoH.Interfaces;

/// <summary>
/// Представляет игровое поле.
/// </summary>
public interface IGameBoard
{
    /// <summary>
    /// Размер игрового поля по ширине (количество столбцов).
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Размер игрового поля по высоте (количество строк).
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Двумерный массив клеток, представляющих игровое поле.
    /// </summary>
    ICell[,] Cells { get; }

    /// <summary>
    /// Проверяет, является ли ячейка доступной для перемещения.
    /// </summary>
    /// <param name="x">Координата X.</param>
    /// <param name="y">Координата Y.</param>
    /// <returns>true, если ячейка доступна для перемещения; иначе false.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если координаты выходят за пределы игрового поля.</exception>
    bool IsCellAvailable(int x, int y);
}
