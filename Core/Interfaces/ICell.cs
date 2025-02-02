namespace BoH.Interfaces;


/// <summary>
/// Представляет клетку игрового поля.
/// </summary>
public interface ICell 
{
    /// <summary>
    /// Позиция клетки на игровом поле.
    /// </summary>
    (int X, int Y) Position { get; }

    /// <summary>
    /// Объект, содержащийся в клетке (юнит, препятствие или ничего).
    /// </summary>
    IIconHolder? Content { get; set; }

    /// <summary>
    /// Проверяет, является ли клетка занятой (содержит юнит или препятствие).
    /// </summary>
    /// <returns>true, если клетка занята; иначе false.</returns>
    bool IsOccupied();

    /// <summary>
    /// Очищает клетку, устанавливая её тип как пустой и удаляя содержимое.
    /// </summary>
    void Clear();

    /// <summary>
    /// Обновляет иконку клетки в зависимости от её содержания
    /// </summary>
    void UpdateIcon();

}
