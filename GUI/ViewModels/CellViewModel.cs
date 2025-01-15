namespace BoH.GUI.ViewModels;

using BoH.Interfaces;

/// <summary>
/// ViewModel, представляющая клетку игрового поля.
/// </summary>
public class CellViewModel
{
    private readonly ICell _cell;

    /// <summary>
    /// Отображение содержимого клетки.
    /// </summary>
    public string Display => _cell.IsOccupied() ? "X" : ".";

    /// <summary>
    /// Позиция клетки на игровом поле.
    /// </summary>
    public (int X, int Y) Position => _cell.Position;

    /// <summary>
    /// Создаёт новый экземпляр <see cref="CellViewModel"/> для указанной клетки.
    /// </summary>
    /// <param name="cell">Модель клетки игрового поля.</param>
    public CellViewModel(ICell cell)
    {
        _cell = cell;
    }
}
