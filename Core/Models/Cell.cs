namespace BoH.Models;

using BoH.Interfaces;

/// <inheritdoc/>
public class Cell : ICell, IIconHolder
{
    /// <inheritdoc/>
    public (int X, int Y) Position { get; }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если символ некорректен (например, не является печатным).
    /// </exception>
    public string Icon
    {
        get => _icon;
        private set
        {
            _icon = value;
        }
    }
    private string _icon = " ";

    /// <inheritdoc/>
    public IIconHolder? Content { get; set; } = null;

    /// <inheritdoc/>
    /// <returns>true, если клетка занята; иначе false.</returns>
    public bool IsOccupied() => Content != null;

    /// <inheritdoc/>
    public void Clear()
    {
        Content = null;
    }

    /// <inheritdoc/>
    public void UpdateIcon()
    {
        Icon = Content?.Icon ?? " ";
    }

    /// <inheritdoc/>
    public void UpdateIcon(string newIcon)
    {
        Icon = newIcon;
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

    public override bool Equals(object? obj)
    {
        if (obj is not Cell other)
            return false;
        return Position.X == other.Position.X && Position.Y == other.Position.Y;
    }

    public override int GetHashCode() => HashCode.Combine(Position.X, Position.Y);
}
