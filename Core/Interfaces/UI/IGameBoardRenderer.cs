namespace BoH.Interfaces;

public interface IGameBoardRenderer
{
    void Render(IGameBoard gameBoard);
    void ScanRender(IGameBoard gameBoard, List<ICell> scannedCells);
}