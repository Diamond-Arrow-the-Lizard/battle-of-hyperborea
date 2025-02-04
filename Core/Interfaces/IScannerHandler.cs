namespace BoH.Interfaces;

public interface IScannerHandler
{
    event Action<IGameBoard, IScanner>? OnGameBoardScanned;
    List<ICell> HandleScan(ICell scanningCell);
}