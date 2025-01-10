namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;
using BoH.Units;

public class Program
{
    public static void Main(string[] args)
    {
        GameBoard gameBoard = new(10, 10);
        GameBoardRenderer.DrawBoard(gameBoard);
    }
}