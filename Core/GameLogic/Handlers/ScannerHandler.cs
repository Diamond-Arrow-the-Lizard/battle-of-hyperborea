namespace BoH.GameLogic;

using BoH.Interfaces;

public class ScannerHandler : IScannerHandler
{
    private IScanner _scanner;
    private IGameBoard _gameBoard;

    public event Action<IGameBoard, IScanner>? OnGameBoardScanned;

    public ScannerHandler(IScanner scanner, IGameBoard gameBoard)
    {
        _scanner = scanner;
        _gameBoard = gameBoard;
    }

    public List<ICell> HandleScan(ICell scanningCell)
    {
        List<ICell> scannedCells = new();
        scannedCells = (List<ICell>)_scanner.Scan(scanningCell, _gameBoard);
        OnGameBoardScanned?.Invoke(_gameBoard, _scanner);
        return scannedCells;
    }
}