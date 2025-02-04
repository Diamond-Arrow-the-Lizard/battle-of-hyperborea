namespace BoH.Interfaces;

/// <summary>
/// Интерфейс обработчика сканирования игрового поля.
/// </summary>
public interface IScannerHandler
{
    /// <summary>
    /// Событие, вызываемое после сканирования игрового поля.
    /// </summary>
    event Action<IGameBoard, List<ICell>>? OnGameBoardScanned;

    /// <summary>
    /// Выполняет сканирование вокруг заданной клетки на указанное расстояние.
    /// </summary>
    /// <param name="scanningCell">Клетка, от которой начинается сканирование.</param>
    /// <param name="range">Радиус сканирования.</param>
    /// <returns>Список отсканированных клеток.</returns>
    List<ICell> HandleScan(ICell scanningCell, int range);
}
