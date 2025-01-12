namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.Units;
using BoH.GameLogic;
using System.ComponentModel;

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
        List<IUnit> teamUnitCells = [];
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                var currentCellContent = board[x, y].Content;
                if (currentCellContent is IUnit unit)
                {
                    if (unit.Team == teamNames[currentTeam])
                    {
                        teamCells.Add(board[x, y]);
                        teamUnitCells.Add(unit);
                        Console.WriteLine(unit.UnitName);
                    }
                }
            }
        }

        Dictionary<(int x, int y), IUnit> teamUnits = new Dictionary<(int, int), IUnit>();
        for (int i = 0; i < teamCells.Count; i++)
        {
            teamUnits[teamCells[i].Position] = teamUnitCells[i];
        }

        while (true)
        {
            int availableUnits = 0;
            Console.WriteLine("Available Units:");
            foreach (var unit in teamUnits)
            {
                if (unit.Value.MadeTurn == true)
                {
                    ++availableUnits;
                    Console.WriteLine($"{unit.Value.UnitName} ({unit.Key.x};{unit.Key.y})");
                }
            }
            if (availableUnits == 0)
            {
                Console.WriteLine("No Units Available");
                break;
            }
            else
            {
                Console.WriteLine("Chosen Unit: ");
                Console.Write("X = ");
                int x = Convert.ToInt32(Console.ReadLine());
                Console.Write("Y = ");
                int y = Convert.ToInt32(Console.ReadLine());
                (int x, int y) userInput = (x, y);
                if (teamUnits.ContainsKey(userInput))
                {
                    var ChosenUnit = teamUnits[userInput];
                    Console.WriteLine($"Chosen unit: {ChosenUnit.UnitName}");
                    Scanner scanner = new Scanner(ChosenUnit.Speed);
                    IEnumerable<ICell> availableMoves = scanner.Scan(board[userInput.x, userInput.y], board);
                    GameBoardRenderer.DrawBoard(board, board[userInput.x, userInput.y], ChosenUnit.Speed);
                    Console.WriteLine($"Available moves: ");
                    foreach (var i in availableMoves) Console.Write($"({i.Position.X};{i.Position.Y}) | ");
                    Console.WriteLine("\nMove unit to...");

                    Console.Write("X = ");
                    int newX = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Y = ");
                    int newY = Convert.ToInt32(Console.ReadLine());
                    Cell move = new((newX, newY));
                    bool isLegal = availableMoves.Any(c => c.Position == move.Position);
                    if (newX >= 0 && newY >= 0 && newX < board.Width && newY < board.Height && board[newX, newY].Content == null && isLegal)
                    {
                        teamUnits[(newX, newY)] = ChosenUnit;
                        board[x, y].Content = null;
                        board[newX, newY].Content = ChosenUnit;
                        teamUnits.Remove(userInput);
                        GameBoardRenderer.DrawBoard(board);
                    }
                    else
                    {
                        Console.WriteLine("Illegal move");
                        continue;
                    }
                    if(ChosenUnit.UnitType == UnitType.Melee)
                    {
                        scanner = new(1);
                        availableMoves = scanner.Scan(board[userInput.x, userInput.y], board);
                        List<IUnit> enemyUnits = [];
                        foreach(var i in availableMoves)
                        {
                            if(i.Content is IUnit unit && unit.Team != ChosenUnit.Team)
                                enemyUnits.Add(unit);
                            
                            if(enemyUnits.Count == 0)
                            {
                                Console.WriteLine("No one to attack");
                                ChosenUnit.MadeTurn = true;
                            }
                            else
                            {
                                GameBoardRenderer.DrawBoard(board, board[newX, newY], 1);
                                //TODO Ударить выбранного юнита
                                ChosenUnit.MadeTurn = true;
                            }
    
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect coordinates");
                    continue;
                }
            }

        }

    }
}