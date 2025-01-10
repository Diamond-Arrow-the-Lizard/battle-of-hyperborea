namespace BoH.Models;

using BoH.Interfaces;

/// <summary>
/// Представляет клетку игрового поля.
/// </summary>
public class Cell : ICell
{
    /// <summary>
    /// Позиция клетки на игровом поле.
    /// </summary>
    public (int X, int Y) Position { get; }

    /// <summary>
    /// Объект, содержащийся в клетке (юнит, препятствие или ничего).
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// Проверяет, является ли клетка занятой (содержит юнит или препятствие).
    /// </summary>
    /// <returns>true, если клетка занята; иначе false.</returns>
    public bool IsOccupied() => Content != null;

    /// <summary>
    /// Очищает клетку, устанавливая её тип как пустой и удаляя содержимое.
    /// </summary>
    public void Clear()
    {
        Content = null;
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Cell"/> с указанной позицией.
    /// </summary>
    /// <param name="Position">Позиция клетки на игровом поле в формате (X, Y).</param>
    /// <exception cref="ArgumentException">Генерируется, если координаты отрицательные.</exception>
    public Cell((int X, int Y) Position)
    {
        if (Position.X < 0 || Position.Y < 0)
        {
            throw new ArgumentException("Координаты клетки не могут быть отрицательными.");
        }
        this.Position = Position;
    }
}
