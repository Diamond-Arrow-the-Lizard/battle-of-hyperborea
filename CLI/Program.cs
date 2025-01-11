namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;
using BoH.Units;

public class Program
{
    public static void Main(string[] args)
    {
        var board = new GameBoard(10, 10);

        board[0, 0].Content = new Obstacle();

        var archer = new RusArcher();
        var warrior = new RusWarrior();
        board[3, 3].Content = archer;
        board[4, 5].Content = warrior;

        Console.WriteLine(warrior.Abilities[0].Activate(warrior, archer));
        Console.WriteLine(archer.Hp);
        Console.WriteLine(archer.IsStunned);
        Console.WriteLine(archer.Abilities[0].Activate(archer));
        Console.WriteLine(archer.Hp);

        // Рендеринг игрового поля
        GameBoardRenderer.DrawBoard(board);

    }
}