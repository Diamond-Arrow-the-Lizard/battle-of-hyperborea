namespace BoH.GameLogic;

using BoH.Interfaces;

/// <summary>
/// Реализация обработчика сканирования игрового поля.
/// </summary>
public class ScannerHandler : IScannerHandler
{
    private IGameBoard _gameBoard;
    
    /// <inheritdoc/>
    public event Action<IGameBoard, List<ICell>>? OnGameBoardScanned;

    /// <summary>
    /// Создает экземпляр обработчика сканирования.
    /// </summary>
    /// <param name="gameBoard">Игровое поле.</param>
    public ScannerHandler(IGameBoard gameBoard)
    {
        _gameBoard = gameBoard;
    }

    /// <inheritdoc/>
    public List<ICell> HandleScan(ICell scanningCell, int range)
    {
        Scanner scanner = new(range);
        List<ICell> scannedCells = new();
        scannedCells = (List<ICell>)scanner.Scan(scanningCell, _gameBoard);
        OnGameBoardScanned?.Invoke(_gameBoard, scannedCells);
        return scannedCells;
    }
}
