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

        int teamSize = GetTeamSize();
        teams = SetupTeams(teamSize);

        IGameBoard board = gameController.StartGame(8, 8, teams);
        GameBoardRenderer.DrawBoard(board);

        while (true)
        {
            if (await HandleGameSave(service)) break;

            List<string> teamNames = teams.Keys.ToList();
            if (currentTeam == 2)
            {
                Console.WriteLine("Game ended.");
                await gameController.EndGame();
                break;
            }
            Console.WriteLine($"Current team: {teamNames[currentTeam]}");

            List<ICell> teamCells = GetTeamCells(teamNames[currentTeam], board);
            List<IUnit> teamUnitCells = GetTeamUnits(teamCells);

            Dictionary<(int x, int y), IUnit> teamUnits = MapUnitsToCells(teamCells, teamUnitCells);

            while (true)
            {
                if (!HandleTurn(teamUnits, board)) break;

                currentTeam = gameController.NextTurn();
            }
        }
    }

    private static int GetTeamSize()
    {
        Console.WriteLine("Teams size: ");
        return Convert.ToInt32(Console.ReadLine());
    }

    private static Dictionary<string, List<IUnit>> SetupTeams(int teamSize)
    {
        Dictionary<string, List<IUnit>> teams = new();
        switch (teamSize)
        {
            case 1:
                Console.WriteLine("Normal");
                teams["Rus"] = new List<IUnit> { new RusArcher(), new RusWarrior(), new RusWarrior() };
                teams["Lizard"] = new List<IUnit> { new LizardArcher(), new LizardWarrior(), new LizardWarrior() };
                break;
            case 2:
                Console.WriteLine("Big");
                teams["Rus"] = new List<IUnit> { new RusArcher(), new RusArcher(), new RusWarrior(), new RusWarrior(), new RusWarrior(), new RusWarrior() };
                teams["Lizard"] = new List<IUnit> { new LizardArcher(), new LizardArcher(), new LizardWarrior(), new LizardWarrior(), new LizardWarrior(), new LizardWarrior() };
                break;
            default:
                throw new Exception("Invalid choice");
        }
        return teams;
    }

    private static async Task<bool> HandleGameSave(GameBoardService service)
    {
        Console.WriteLine("Save Game? (y/n)");
        string? ans = Console.ReadLine();
        if (ans != null && ans[0] == 'y')
        {
            await service.SaveGameBoardAsync();
            return true;
        }
        return false;
    }

    private static List<ICell> GetTeamCells(string teamName, IGameBoard board)
    {
        List<ICell> teamCells = new();
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                var currentCellContent = board[x, y].Content;
                if (currentCellContent is IUnit unit && unit.Team == teamName)
                {
                    teamCells.Add(board[x, y]);
                }
            }
        }
        return teamCells;
    }

    private static List<IUnit> GetTeamUnits(List<ICell> teamCells)
    {
        List<IUnit> teamUnitCells = new();
        foreach (var cell in teamCells)
        {
            if (cell.Content is IUnit unit)
            {
                teamUnitCells.Add(unit);
            }
        }
        return teamUnitCells;
    }

    private static Dictionary<(int x, int y), IUnit> MapUnitsToCells(List<ICell> teamCells, List<IUnit> teamUnitCells)
    {
        Dictionary<(int x, int y), IUnit> teamUnits = new();
        for (int i = 0; i < teamCells.Count; i++)
        {
            teamUnits[teamCells[i].Position] = teamUnitCells[i];
        }
        return teamUnits;
    }

    private static bool HandleTurn(Dictionary<(int x, int y), IUnit> teamUnits, IGameBoard board)
    {
        int availableUnits = 0;
        Console.WriteLine("Available Units:");
        foreach (var unit in teamUnits)
        {
            if (!unit.Value.MadeTurn)
            {
                ++availableUnits;
                Console.WriteLine($"{unit.Value.UnitName} ({unit.Key.x};{unit.Key.y})");
            }
        }
        if (availableUnits == 0)
        {
            Console.WriteLine("No units available, turn ends.");
            return false;
        }

        return HandleUnitAction(teamUnits, board);
    }

    private static bool HandleUnitAction(Dictionary<(int x, int y), IUnit> teamUnits, IGameBoard board)
    {
        Console.WriteLine("Chosen Unit: ");
        Console.Write("X = ");
        int x = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int y = Convert.ToInt32(Console.ReadLine());
        (int x, int y) userInput = (x, y);

        if (!teamUnits.ContainsKey(userInput))
        {
            Console.WriteLine("Incorrect coordinates");
            return true;
        }

        var ChosenUnit = teamUnits[userInput];
        if (ChosenUnit.MadeTurn)
        {
            Console.WriteLine("Unit has already made his turn");
            return true;
        }

        Console.WriteLine($"Chosen unit: {ChosenUnit.UnitName}");
        return HandleUnitMoveAndAttack(ChosenUnit, board, userInput, teamUnits);
    }

    private static bool HandleUnitMoveAndAttack(IUnit ChosenUnit, IGameBoard board, (int x, int y) userInput, Dictionary<(int x, int y), IUnit> teamUnits)
    {
        Scanner scanner = new(ChosenUnit.Speed);
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

        if (newX < 0 || newY < 0 || newX >= board.Width || newY >= board.Height || board[newX, newY].Content != null || !isLegal)
        {
            Console.WriteLine("Illegal move");
            return true;
        }

        teamUnits[(newX, newY)] = ChosenUnit;
        board[userInput.x, userInput.y].Content = null;
        board[newX, newY].Content = ChosenUnit;
        teamUnits.Remove(userInput);
        GameBoardRenderer.DrawBoard(board);

        return HandleAttackOrAbility(ChosenUnit, board, userInput, newX, newY);
    }

    private static bool HandleAttackOrAbility(IUnit ChosenUnit, IGameBoard board, (int x, int y) userInput, int newX, int newY)
    {
        Scanner scanner = new(ChosenUnit.Range);
        IEnumerable<ICell> availableEnemies = scanner.Scan(board[userInput.x, userInput.y], board);
        Dictionary<(int x, int y), IUnit> enemyUnits = new();
        foreach (var i in availableEnemies)
        {
            if (i.Content is IUnit unit && unit.Team != ChosenUnit.Team)
                enemyUnits[i.Position] = unit;
        }

        if (enemyUnits.Count == 0)
        {
            Console.WriteLine("No one to attack");
            ChosenUnit.MadeTurn = true;
            return false;
        }

        GameBoardRenderer.DrawBoard(board, board[newX, newY], ChosenUnit.Range);
        Console.WriteLine("Should Unit Attack? ==> ");
        int userChoice = Convert.ToInt32(Console.ReadLine());

        switch (userChoice)
        {
            case 1:
                return HandleAttack(board, ChosenUnit, enemyUnits);
            case 2:
                return HandleAbility(board, ChosenUnit, enemyUnits);
            case 3:
                Console.WriteLine("Unit stayed in place");
                ChosenUnit.MadeTurn = true;
                return false;
            default:
                Console.WriteLine("Invalid choice");
                return true;
        }
    }

    private static bool HandleAttack(IGameBoard board, IUnit ChosenUnit, Dictionary<(int x, int y), IUnit> enemyUnits)
    {
        Console.WriteLine("Enemies available to hit:");
        foreach (var j in enemyUnits)
            Console.WriteLine($"{j.Value.UnitName} ({j.Key.x};{j.Key.y})");

        Console.WriteLine("Chosen Unit: ");
        Console.Write("X = ");
        int enemyX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int enemyY = Convert.ToInt32(Console.ReadLine());
        (int enemyX, int enemyY) attackedEnemy = (enemyX, enemyY);

        if (!enemyUnits.ContainsKey(attackedEnemy))
        {
            Console.WriteLine("Illegal move");
            return true;
        }

        Console.WriteLine("Attacking enemy");
        if (board[enemyX, enemyY].Content is IUnit target && target.Team != ChosenUnit.Team)
        {
            ChosenUnit.Attack(target);
            Console.WriteLine($"{target.UnitName}'s HP reduced to {target.Hp}");
            ChosenUnit.MadeTurn = true;
        }
        return false;
    }

    private static bool HandleAbility(IGameBoard board, IUnit ChosenUnit, Dictionary<(int x, int y), IUnit> enemyUnits)
    {
        List<IAbility> activeAbilities = ChosenUnit.Abilities.FindAll(x => x.IsActive == true);
        if (activeAbilities.Count == 0)
        {
            Console.WriteLine("Unit has no active abilities");
            return false;
        }

        int index = 1;
        foreach (var k in activeAbilities)
        {
            Console.WriteLine($"{index}. {k.Name} - {k.Description}");
            ++index;
        }
        Console.Write("Choose Ability ==> ");
        int chosenAbility = Convert.ToInt32(Console.ReadLine());
        if (chosenAbility < 1 || chosenAbility > activeAbilities.Count)
        {
            Console.WriteLine("Invalid choice");
            return true;
        }

        IAbility Ability = activeAbilities[chosenAbility - 1];
        Console.WriteLine("Enemies available to hit:");
        foreach (var j in enemyUnits)
            Console.WriteLine($"{j.Value.UnitName} ({j.Key.x};{j.Key.y})");

        Console.WriteLine("Chosen Unit: ");
        Console.Write("X = ");
        int enemyX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int enemyY = Convert.ToInt32(Console.ReadLine());
        (int enemyX, int enemyY) attackedEnemy = (enemyX, enemyY);

        if (!enemyUnits.ContainsKey(attackedEnemy))
        {
            Console.WriteLine("Illegal move");
            return true;
        }

        Console.WriteLine("Activating ability");
        if (board[enemyX, enemyY].Content is IUnit target && target.Team != ChosenUnit.Team)
        {
            if (Ability.Activate(ChosenUnit, target))
            {
                Console.WriteLine($"{target.UnitName}'s HP reduced to {target.Hp}");
                ChosenUnit.MadeTurn = true;
            }
        }
        return false;
    }
}
