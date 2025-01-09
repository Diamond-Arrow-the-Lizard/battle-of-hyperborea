namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;
using BoH.Units;

public class Program
{
    public static void Main(string[] args)
    {
        GameBoard gameBoard = new(10, 10);
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine();
            for (int j = 0; j < 10; j++)
            {
                var currentCell = gameBoard[i, j];
                IUnit unit = new BaseUnit();
                currentCell.Content = unit;
                if (currentCell.Content == null)
                {
                    Console.Write("O ");
                }
                else if (currentCell.Content.GetType() == typeof(BaseUnit))
                {
                    Console.Write($"{unit.Icon} ");
                }
            }
        }
    }
}