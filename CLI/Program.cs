namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.Units;
using Microsoft.VisualBasic;

public class Program
{
    public static void Main(string[] args)
    {
        GameBoardService service = new GameBoardService();

        var rusArcher = new RusArcher();
        var rusWarrior = new RusWarrior();

        var lizArcher = new LizardArcher();
        var lizWarrior = new LizardWarrior();

        Dictionary<string, List<IUnit>> teams = new();
        teams["Rus"] = [rusArcher, rusWarrior];
        teams["Lizard"] = [lizArcher, lizWarrior];
        
        IGameBoard board = service.GenerateGameBoard(10, 10, teams);

        // Рендеринг игрового поля
        GameBoardRenderer.DrawBoard(board);

    }
}