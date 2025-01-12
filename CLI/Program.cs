namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.Units;
using BoH.GameLogic;

public class Program
{
    public static async Task Main(string[] args)
    {
        GameBoardService service = new GameBoardService();
        GameController gameController = new GameController(service);
        int currentTeam = 0;

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

        var board = gameController.StartGame(5, 5, teams);

        GameBoardRenderer.DrawBoard(board);
        while (true)
        {
            Console.WriteLine("Save Game? (y/n)");
            string? ans = "";
            ans = Console.ReadLine();
            if (ans != null)
            {
                switch (ans[0])
                {
                    case 'y':
                        await service.SaveGameBoardAsync();
                        break;
                    default:
                        break;
                }

            }
            List<string> teamNames = teams.Keys.ToList();
            if(currentTeam == 2)
            {
                Console.WriteLine("Game ended.");
                await gameController.EndGame();
                break;
            }
            Console.WriteLine($"Current team: {teamNames[currentTeam]}");

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
                    if (unit.Value.MadeTurn == false)
                    {
                        ++availableUnits;
                        Console.WriteLine($"{unit.Value.UnitName} ({unit.Key.x};{unit.Key.y})");
                    }
                }
                if (availableUnits == 0)
                {
                    Console.WriteLine("No units available, turn ends.");
                    currentTeam = gameController.NextTurn();
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
                        if (ChosenUnit.MadeTurn == true)
                        {
                            Console.WriteLine("Unit has already made his turn");
                            continue;
                        }
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
                        scanner = new(ChosenUnit.Range);
                        IEnumerable<ICell> availableEnemies = scanner.Scan(board[userInput.x, userInput.y], board);
                        Dictionary<(int x, int y), IUnit> enemyUnits = new();
                        foreach (var i in availableEnemies)
                        {
                            if (i.Content is IUnit unit && unit.Team != ChosenUnit.Team)
                                enemyUnits[i.Position] = unit;

                            if (enemyUnits.Count == 0)
                            {
                                Console.WriteLine("No one to attack");
                                ChosenUnit.MadeTurn = true;
                                continue;
                            }
                            else
                            {
                                GameBoardRenderer.DrawBoard(board, board[newX, newY], ChosenUnit.Range);
                                Console.WriteLine("Should Unit Attack? ==> ");
                                int userChoice = Convert.ToInt32(Console.ReadLine());
                                switch (userChoice)
                                {
                                    case 1: // Атака
                                        Console.WriteLine("Enemies available to hit:");
                                        foreach (var j in enemyUnits)
                                            Console.WriteLine($"{j.Value.UnitName} ({j.Key.x};{j.Key.y})");

                                        Console.WriteLine("Chosen Unit: ");
                                        Console.Write("X = ");
                                        int enemyX = Convert.ToInt32(Console.ReadLine());
                                        Console.Write("Y = ");
                                        int enemyY = Convert.ToInt32(Console.ReadLine());
                                        (int enemyX, int enemyY) attackedEnemy = (enemyX, enemyY);

                                        // Проверяем, что координаты корректны и содержат врага
                                        isLegal = enemyUnits.Any(x => x.Key == attackedEnemy);
                                        if (enemyX >= 0 && enemyY >= 0 && enemyX < board.Width && enemyY < board.Height && isLegal)
                                        {
                                            Console.WriteLine("Attacking enemy");
                                            if (board[enemyX, enemyY].Content is IUnit target && target.Team != ChosenUnit.Team)
                                            {
                                                ChosenUnit.Attack(target);
                                                Console.WriteLine($"{target.UnitName}'s HP reduced to {target.Hp}");
                                                ChosenUnit.MadeTurn = true;

                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Illegal move");
                                        }
                                        break;


                                    case 2: // Активировать способность
                                        List<IAbility> activeAbilities = ChosenUnit.Abilities.FindAll(x => x.IsActive == true);
                                        if (activeAbilities.Count == 0)
                                        {
                                            Console.WriteLine("Unit has no active abilities");
                                            break;
                                        }
                                        else
                                        {
                                            int index = 1;
                                            foreach (var k in activeAbilities)
                                            {
                                                Console.WriteLine($"{index}. {k.Name} - {k.Description}");
                                                ++index;
                                            }
                                            Console.Write("Choose Ability ==>");
                                            int chosenAbility = Convert.ToInt32(Console.ReadLine());
                                            if (activeAbilities.Count < index - 1 || index - 1 < 0)
                                            {
                                                Console.WriteLine("Invalid choice");
                                            }
                                            else
                                            {
                                                IAbility Ability = activeAbilities[index];
                                                Console.WriteLine("Enemies available to hit:");
                                                foreach (var j in enemyUnits)
                                                    Console.WriteLine($"{j.Value.UnitName} ({j.Key.x};{j.Key.y})");

                                                Console.WriteLine("Chosen Unit: ");
                                                Console.Write("X = ");
                                                enemyX = Convert.ToInt32(Console.ReadLine());
                                                Console.Write("Y = ");
                                                enemyY = Convert.ToInt32(Console.ReadLine());
                                                attackedEnemy = (enemyX, enemyY);

                                                // Проверяем, что координаты корректны и содержат врага
                                                isLegal = enemyUnits.Any(x => x.Key == attackedEnemy);
                                                if (enemyX >= 0 && enemyY >= 0 && enemyX < board.Width && enemyY < board.Height && isLegal)
                                                {
                                                    Console.WriteLine("Attacking enemy");
                                                    if (board[enemyX, enemyY].Content is IUnit target && target.Team != ChosenUnit.Team)
                                                    {
                                                        if(Ability.Activate(ChosenUnit, target))
                                                        {
                                                        Console.WriteLine($"{target.UnitName}'s HP reduced to {target.Hp}");
                                                        ChosenUnit.MadeTurn = true;

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Illegal move");
                                                }
                                            }
                                        }
                                        break;

                                    case 3: // пропустить ход
                                        Console.WriteLine("Unit stayed in place");
                                        ChosenUnit.MadeTurn = true;
                                        break;

                                    default:
                                        Console.WriteLine("Invalid choice");
                                        break;
                                };

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
}