namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.Units;
using BoH.GameLogic;

public class Program
{
    public static void Main(string[] args)
    {
        GameBoardService service = new GameBoardService();
        GameController gameController = new GameController(service);

        Dictionary<string, List<IUnit>> teams = new Dictionary<string, List<IUnit>>();

        Console.WriteLine("Teams size: ");
        int teamSize = Convert.ToInt32(Console.ReadLine());

        switch (teamSize)
        {
            case 1:
                Console.WriteLine("Normal");
                teams["Rus"] = [new RusArcher(), new RusWarrior(), new RusWarrior()];
                teams["Lizard"] = [new LizardArcher(), new LizardWarrior(), new LizardWarrior()];
                break;
            case 2:
                Console.WriteLine("Big");
                teams["Rus"] = [new RusArcher(), new RusArcher(), new RusWarrior(), new RusWarrior(), new RusWarrior(), new RusWarrior()];
                teams["Lizard"] = [new LizardArcher(), new LizardArcher(), new LizardWarrior(), new LizardWarrior(), new LizardWarrior(), new LizardWarrior()];
                break;
            default:
                throw new Exception("Invalid choice");
        }

        var board = gameController.StartGame(10, 10, teams);

        GameBoardRenderer.DrawBoard(board);

        List<string> teamNames = teams.Keys.ToList();
        int currentTeam = 0;
        Console.WriteLine($"Current team: {teamNames[currentTeam]}");

        Console.WriteLine("Unit positions:");
        // Позиции юнитов команды
        List<ICell> teamCells = [];
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                var currentCellContent = board[x, y].Content;
                if(currentCellContent is IUnit unit)
                {
                    if(unit.Team == teamNames[currentTeam])
                    teamCells.Add(board[x, y]);
                }
            }
        }
        foreach(var i in teamCells) Console.WriteLine(i.Position);

        

    }
}