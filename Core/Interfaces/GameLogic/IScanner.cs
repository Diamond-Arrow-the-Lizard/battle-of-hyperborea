namespace BoH.Interfaces;

using System.Collections.Generic;

/// <summary>
/// Интерфейс для объекта, способного сканировать клетки игрового поля вокруг заданной клетки.
/// </summary>
public interface IScanner
{
    /// <summary>
    /// Радиус сканирования в клетках.
    /// </summary>
    int Range { get; }

    /// <summary>
    /// Сканирует клетки вокруг заданной клетки на игровом поле в пределах радиуса.
    /// </summary>
    /// <param name="scanningCell">Клетка, вокруг которой производится сканирование.</param>
    /// <param name="gameBoard">Игровое поле, на котором производится сканирование.</param>
    /// <returns>Коллекция клеток в пределах радиуса вокруг заданной клетки.</returns>
    List<ICell> Scan(ICell scanningCell, IGameBoard gameBoard);
}
