namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.Units;
using BoH.GameLogic;

/// <summary>
/// Главная точка входа в игру.
/// </summary>
public class Program
{
    /// <summary>
    /// Основной метод программы, инициирующий игровой процесс.
    /// </summary>
    public static async Task Main(string[] args)
    {
        var service = new GameBoardService();
        var gameController = new GameController(service);

        int currentTeam = 0;
        var teams = InitializeTeams();

        var board = gameController.StartGame(5, 5, teams);
        GameBoardRenderer.DrawBoard(board);

        while (true)
        {
            // Запрос на сохранение игры
            if (await PromptSaveGame(service))
                continue;

            // Проверка завершения игры
            if (IsGameOver(teams, currentTeam))
                break;

            // Исполнение хода команды
            if (board is GameBoard gameBoard)
                ExecuteTeamTurn(gameBoard, teams, gameController, ref currentTeam);
        }
    }

    /// <summary>
    /// Инициализирует команды в игре на основе ввода пользователя.
    /// Запрашивает размер команды и создаёт юнитов для каждой команды.
    /// </summary>
    /// <returns>Словарь команд, где ключ — это название команды, а значение — список юнитов.</returns>
    private static Dictionary<string, List<IUnit>> InitializeTeams()
    {
        var teams = new Dictionary<string, List<IUnit>>();

        Console.WriteLine("Введите размер команды (1 - нормальная, 2 - большая):");
        int teamSize = Convert.ToInt32(Console.ReadLine());

        switch (teamSize)
        {
            case 1:
                Console.WriteLine("Выбрана нормальная команда.");
                // Создание нормальной команды
                teams["Rus"] = new List<IUnit> { new RusArcher(), new RusWarrior(), new RusWarrior() };
                teams["Lizard"] = new List<IUnit> { new LizardArcher(), new LizardWarrior(), new LizardWarrior() };
                break;
            case 2:
                Console.WriteLine("Выбрана большая команда.");
                // Создание большой команды
                teams["Rus"] = new List<IUnit> { new RusArcher(), new RusArcher(), new RusWarrior(), new RusWarrior(), new RusWarrior(), new RusWarrior() };
                teams["Lizard"] = new List<IUnit> { new LizardArcher(), new LizardArcher(), new LizardWarrior(), new LizardWarrior(), new LizardWarrior(), new LizardWarrior() };
                break;
            default:
                throw new Exception("Недопустимый выбор.");
        }

        return teams;
    }

    /// <summary>
    /// Запрашивает у пользователя, хочет ли он сохранить текущую игру.
    /// </summary>
    /// <param name="service">Сервис для сохранения состояния игры.</param>
    /// <returns>Возвращает true, если пользователь выбрал сохранить игру, иначе false.</returns>
    private static async Task<bool> PromptSaveGame(GameBoardService service)
    {
        Console.WriteLine("Сохранить игру? (y/n)");
        string? ans = Console.ReadLine();

        if (ans != null && ans.ToLower() == "y")
        {
            await service.SaveGameBoardAsync();
            Console.WriteLine("Игра сохранена.");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Проверяет, завершена ли игра.
    /// Игра считается завершённой, если все команды совершили свой ход.
    /// </summary>
    /// <param name="teams">Словарь команд.</param>
    /// <param name="currentTeam">Индекс текущей команды, чей ход наступил.</param>
    /// <returns>Возвращает true, если игра завершена, иначе false.</returns>
    private static bool IsGameOver(Dictionary<string, List<IUnit>> teams, int currentTeam)
    {
        if (currentTeam >= teams.Count)
        {
            Console.WriteLine("Игра завершена.");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Исполняет ход текущей команды.
    /// Выполняет действия для каждого юнита команды, пока все юниты не сделают ход.
    /// </summary>
    /// <param name="board">Игровое поле, представляющее текущее состояние игры.</param>
    /// <param name="teams">Словарь команд.</param>
    /// <param name="gameController">Контроллер игры, управляющий её логикой.</param>
    /// <param name="currentTeam">Ссылка на индекс текущей команды.</param>
    /// <returns>Асинхронная задача выполнения хода.</returns>
    private static void ExecuteTeamTurn(GameBoard board, Dictionary<string, List<IUnit>> teams, GameController gameController, ref int currentTeam)
    {
        List<string> teamNames = teams.Keys.ToList();

        Console.WriteLine($"Ход команды: {teamNames[currentTeam]}");

        var teamUnits = GetTeamUnits(board, teamNames[currentTeam]);

        while (true)
        {
            if (HandleUnitTurn(board, teamUnits, currentTeam, gameController))
                break;
        }

        currentTeam = gameController.NextTurn();
    }

    /// <summary>
    /// Получает всех юнитов, принадлежащих конкретной команде, с учётом их расположения на поле.
    /// </summary>
    /// <param name="board">Игровое поле.</param>
    /// <param name="teamName">Название команды.</param>
    /// <returns>Словарь, где ключ — это координаты на поле, а значение — это юнит.</returns>
    private static Dictionary<(int x, int y), IUnit> GetTeamUnits(GameBoard board, string teamName)
    {
        var teamUnits = new Dictionary<(int x, int y), IUnit>();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                var content = board[x, y].Content;
                if (content is IUnit unit && unit.Team == teamName)
                {
                    teamUnits[(x, y)] = unit;
                }
            }
        }

        return teamUnits;
    }

    /// <summary>
    /// Обрабатывает ход конкретного юнита, позволяя ему выполнить действия, такие как движение.
    /// </summary>
    /// <param name="board">Игровое поле.</param>
    /// <param name="teamUnits">Словарь юнитов команды с координатами.</param>
    /// <param name="currentTeam">Индекс текущей команды.</param>
    /// <param name="gameController">Контроллер игры, управляющий её логикой.</param>
    /// <returns>Возвращает true, если ход завершён, иначе false.</returns>
    private static bool HandleUnitTurn(GameBoard board, Dictionary<(int x, int y), IUnit> teamUnits, int currentTeam, GameController gameController)
    {
        var availableUnits = teamUnits.Where(unit => !unit.Value.MadeTurn).ToList();

        if (!availableUnits.Any())
        {
            Console.WriteLine("Нет доступных юнитов. Ход завершён.");
            return true;
        }

        Console.WriteLine("Доступные юниты:");
        foreach (var unit in availableUnits)
        {
            Console.WriteLine($"{unit.Value.UnitName} ({unit.Key.x}, {unit.Key.y}), HP: {unit.Value.Hp}, Speed: {unit.Value.Speed}, Range: {unit.Value.Range}");
        }

        Console.WriteLine("Выберите юнита: Введите координаты.");
        Console.Write("X = ");
        int x = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int y = Convert.ToInt32(Console.ReadLine());
        var userInput = (x, y);

        if (!teamUnits.ContainsKey(userInput))
        {
            Console.WriteLine("Некорректные координаты. Повторите попытку.");
            return false;
        }

        var chosenUnit = teamUnits[userInput];

        if (chosenUnit.MadeTurn)
        {
            Console.WriteLine("Этот юнит уже сделал ход.");
            return false;
        }

        Console.WriteLine($"Выбран юнит: {chosenUnit.UnitName}, HP: {chosenUnit.Hp}, Speed: {chosenUnit.Speed}, Range: {chosenUnit.Range}");

        return HandleUnitMovement(board, teamUnits, userInput, chosenUnit);
    }

    /// <summary>
    /// Обрабатывает перемещение выбранного юнита.
    /// Позволяет выбрать координаты для движения юнита на поле.
    /// </summary>
    /// <param name="board">Игровое поле.</param>
    /// <param name="teamUnits">Словарь юнитов команды с координатами.</param>
    /// <param name="userInput">Координаты выбранного юнита.</param>
    /// <param name="chosenUnit">Выбранный юнит.</param>
    /// <returns>Возвращает true, если перемещение успешно, иначе false.</returns>
    private static bool HandleUnitMovement(GameBoard board, Dictionary<(int x, int y), IUnit> teamUnits, (int x, int y) userInput, IUnit chosenUnit)
    {
        var scanner = new Scanner(chosenUnit.Speed);
        var availableMoves = scanner.Scan(board[userInput.x, userInput.y], board);

        Console.WriteLine("Доступные ходы:");
        foreach (var cell in availableMoves)
        {
            Console.Write($"({cell.Position.X}, {cell.Position.Y}) ");
        }
        Console.WriteLine();

        Console.WriteLine("Переместите юнита: Введите координаты.");
        Console.Write("X = ");
        int newX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int newY = Convert.ToInt32(Console.ReadLine());

        if (TryMoveUnit(board, teamUnits, userInput, (newX, newY), chosenUnit))
        {
            Console.WriteLine("Юнит перемещён.");
            return true;
        }
        else
        {
            Console.WriteLine("Недопустимый ход.");
            return false;
        }
    }

    /// <summary>
    /// Пытается переместить юнита на новое место на поле, проверяя допустимость перемещения.
    /// </summary>
    /// <param name="board">Игровое поле.</param>
    /// <param name="teamUnits">Словарь юнитов команды с координатами.</param>
    /// <param name="oldPosition">Старые координаты юнита.</param>
    /// <param name="newPosition">Новые координаты юнита.</param>
    /// <param name="chosenUnit">Выбранный юнит для перемещения.</param>
    /// <returns>Возвращает true, если перемещение успешно, иначе false.</returns>
    private static bool TryMoveUnit(GameBoard board, Dictionary<(int x, int y), IUnit> teamUnits, (int x, int y) oldPosition, (int x, int y) newPosition, IUnit chosenUnit)
    {
        var scanner = new Scanner(chosenUnit.Speed);
        var availableMoves = scanner.Scan(board[oldPosition.x, oldPosition.y], board);

        if (newPosition.x >= 0 && newPosition.y >= 0 && newPosition.x < board.Width && newPosition.y < board.Height &&
            availableMoves.Any(cell => cell.Position == newPosition) && board[newPosition.x, newPosition.y].Content == null)
        {
            board[oldPosition.x, oldPosition.y].Content = null;
            board[newPosition.x, newPosition.y].Content = chosenUnit;

            teamUnits.Remove(oldPosition);
            teamUnits[newPosition] = chosenUnit;
            return true;
        }

        return false;
    }
}
