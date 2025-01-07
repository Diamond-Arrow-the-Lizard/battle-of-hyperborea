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
    /// Возвращает или задаёт содержимое ячейки на игровом поле.
    /// </summary>
    /// <param name="x">Координата X (столбец).</param>
    /// <param name="y">Координата Y (строка).</param>
    /// <returns>Тип содержимого клетки (пусто, юнит, препятствие).</returns>
    CellType GetCellType(int x, int y);

    /// <summary>
    /// Возвращает или задаёт объект, расположенный на клетке.
    /// </summary>
    /// <param name="x">Координата X (столбец).</param>
    /// <param name="y">Координата Y (строка).</param>
    /// <returns>Юнит или null, если клетка пуста или содержит препятствие.</returns>
    IUnit? this[int x, int y] { get; set; }

    /// <summary>
    /// Проверяет, является ли ячейка доступной для перемещения.
    /// </summary>
    /// <param name="x">Координата X.</param>
    /// <param name="y">Координата Y.</param>
    /// <returns>true, если ячейка доступна для перемещения (пустая или с юнитом); иначе false.</returns>
    bool IsCellAvailable(int x, int y);
}

