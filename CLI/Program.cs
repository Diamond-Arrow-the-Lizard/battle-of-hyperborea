namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;
using BoH.Units;

public class Program
{
    public static void Main(string[] args)
    {
        GameBoard gameBoard = new(10, 10);
        gameBoard[0, 2].CellType = CellType.Obstacle;
        gameBoard[8, 3].CellType = CellType.Obstacle;
        gameBoard[3, 4].CellType = CellType.Obstacle;
        GameBoardRenderer.DrawBoard(gameBoard);
    }
}