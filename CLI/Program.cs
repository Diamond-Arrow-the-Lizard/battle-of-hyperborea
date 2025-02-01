namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Services;
using BoH.Models;
using BoH.GameLogic;

public class Program
{
    /// <summary>
    /// Точка входа в приложение. Отвечает за запуск игры и обработку игровых раундов.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    public static async Task Main(string[] args)
    {
        // Инициализация игрового сервиса и контроллера
        GameBoardService service = new GameBoardService();
        GameController gameController = new GameController(service);
        int currentTeam = 0;

        // Словарь для хранения команд
        Dictionary<string, List<IUnit>> teams = new Dictionary<string, List<IUnit>>();

        // Установка размера команд
        int teamSize = GetTeamSize();
        teams = SetupTeams(teamSize);

        // Создание игрового поля и отрисовка
        IGameBoard board = gameController.StartGame(8, 8, teams);
        GameBoardRenderer.DrawBoard(board);

        // Основной цикл игры
        while (true)
        {
            // Проверка необходимости сохранения игры
            if (await HandleGameSave(service))
                break;

            // Получение списка имен команд
            List<string> teamNames = teams.Keys.ToList();

            // Проверка на окончание игры
            if (currentTeam == 2)
            {
                Console.WriteLine("Игра завершена.");
                await gameController.EndGame();
                break;
            }

            Console.WriteLine($"Текущая команда: {teamNames[currentTeam]}");

            // Получение клеток и юнитов текущей команды
            List<ICell> teamCells = GetTeamCells(teamNames[currentTeam], board);
            List<IUnit> teamUnitCells = GetTeamUnits(teamCells);

            // Сопоставление клеток и юнитов
            Dictionary<(int x, int y), IUnit> teamUnits = MapUnitsToCells(teamCells, teamUnitCells);

            bool teamFinishedTurn = false;

            // Цикл хода команды
            while (!teamFinishedTurn)
            {
                teamFinishedTurn = !HandleTurn(teamUnits, board);

                // Проверка: все ли юниты команды завершили свои ходы
                if (teamUnits.Values.All(u => u.MadeTurn))
                {
                    Console.WriteLine("Все юниты команды завершили ход.");
                    currentTeam = gameController.NextTurn();
                    teamFinishedTurn = true; // Завершаем текущий цикл команды
                }
            }
        }

    }


    /// <summary>
    /// Запрашивает у пользователя размер команды и возвращает его.
    /// </summary>
    /// <returns>Размер команды (1 или 2).</returns>
    private static int GetTeamSize()
    {
        while (true)
        {
            Console.WriteLine("Введите размер команды (1 - Normal, 2 - Big):");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int teamSize) && (teamSize == 1 || teamSize == 2))
            {
                return teamSize;
            }
            Console.WriteLine("Некорректный ввод. Пожалуйста, введите 1 или 2.");
        }
    }

    /// <summary>
    /// Настраивает команды в зависимости от их размера.
    /// </summary>
    /// <param name="teamSize">Размер команды (1 - Normal, 2 - Big).</param>
    /// <returns>Словарь с командами и их юнитами.</returns>
    private static Dictionary<string, List<IUnit>> SetupTeams(int teamSize)
    {
        Dictionary<string, List<IUnit>> teams = new();

        switch (teamSize)
        {
            case 1:
                Console.WriteLine("Выбран режим: Normal");
                teams["Rus"] = new List<IUnit> { new RusArcher(), new RusWarrior(), new RusWarrior() };
                teams["Lizard"] = new List<IUnit> { new LizardArcher(), new LizardWarrior(), new LizardWarrior() };
                break;

            case 2:
                Console.WriteLine("Выбран режим: Big");
                teams["Rus"] = new List<IUnit>
            {
                new RusArcher(), new RusArcher(),
                new RusWarrior(), new RusWarrior(),
                new RusWarrior(), new RusWarrior()
            };
                teams["Lizard"] = new List<IUnit>
            {
                new LizardArcher(), new LizardArcher(),
                new LizardWarrior(), new LizardWarrior(),
                new LizardWarrior(), new LizardWarrior()
            };
                break;

            default:
                throw new Exception("Недопустимый размер команды.");
        }

        return teams;
    }

    /// <summary>
    /// Запрашивает у пользователя, нужно ли сохранить игру, и выполняет сохранение.
    /// </summary>
    /// <param name="service">Сервис для работы с игровым полем.</param>
    /// <returns>True, если игра была сохранена, иначе False.</returns>
    private static async Task<bool> HandleGameSave(GameBoardService service)
    {
        Console.WriteLine("Сохранить игру? (y/n):");
        string? input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input) && input.Trim().ToLower().StartsWith("y"))
        {
            await service.SaveGameBoardAsync();
            Console.WriteLine("Игра успешно сохранена.");
            return true;
        }

        Console.WriteLine("Игра не была сохранена.");
        return false;
    }


    /// <summary>
    /// Получает список клеток игрового поля, содержащих юниты указанной команды.
    /// </summary>
    /// <param name="teamName">Название команды.</param>
    /// <param name="board">Игровое поле.</param>
    /// <returns>Список клеток с юнитами команды.</returns>
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

    /// <summary>
    /// Получает список юнитов, расположенных в указанных клетках.
    /// </summary>
    /// <param name="teamCells">Список клеток команды.</param>
    /// <returns>Список юнитов команды.</returns>
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

    /// <summary>
    /// Сопоставляет координаты клеток с соответствующими юнитами.
    /// </summary>
    /// <param name="teamCells">Список клеток команды.</param>
    /// <param name="teamUnitCells">Список юнитов команды.</param>
    /// <returns>Словарь, где ключом является координата клетки, а значением - юнит.</returns>
    private static Dictionary<(int x, int y), IUnit> MapUnitsToCells(List<ICell> teamCells, List<IUnit> teamUnitCells)
    {
        Dictionary<(int x, int y), IUnit> teamUnits = new();
        for (int i = 0; i < teamCells.Count; i++)
        {
            teamUnits[teamCells[i].Position] = teamUnitCells[i];
        }
        return teamUnits;
    }

    /// <summary>
    /// Обрабатывает ход текущей команды.
    /// </summary>
    /// <param name="teamUnits">Словарь юнитов команды с их координатами.</param>
    /// <param name="board">Игровое поле.</param>
    /// <returns>True, если ход продолжается; False, если ход команды завершён.</returns>
    private static bool HandleTurn(Dictionary<(int x, int y), IUnit> teamUnits, IGameBoard board)
    {
        int availableUnits = 0;
        Console.WriteLine("Доступные юниты:");
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
            Console.WriteLine("У всех юнитов команды закончился ход.");
            return false;
        }

        return HandleUnitAction(teamUnits, board);
    }

    /// <summary>
    /// Обрабатывает действие выбранного юнита команды.
    /// </summary>
    /// <param name="teamUnits">Словарь юнитов команды с их координатами.</param>
    /// <param name="board">Игровое поле.</param>
    /// <returns>True, если действия продолжаются; False, если действия завершены.</returns>
    private static bool HandleUnitAction(Dictionary<(int x, int y), IUnit> teamUnits, IGameBoard board)
    {
        Console.WriteLine("Выберите юнита. Введите его координаты.");
        Console.Write("X = ");
        int x = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int y = Convert.ToInt32(Console.ReadLine());
        (int x, int y) userInput = (x, y);

        if (!teamUnits.ContainsKey(userInput))
        {
            Console.WriteLine("Координаты указаны неверно.");
            return true;
        }

        var chosenUnit = teamUnits[userInput];
        if (chosenUnit.MadeTurn)
        {
            Console.WriteLine("Этот юнит уже завершил свой ход.");
            return true;
        }

        Console.WriteLine($"Выбран юнит: {chosenUnit.UnitName}: Здоровье: {chosenUnit.Hp}, Защита: {chosenUnit.Defence}, Скорость: {chosenUnit.Speed}");
        return HandleUnitMoveAndAttack(chosenUnit, board, userInput, teamUnits);
    }

    /// <summary>
    /// Обрабатывает перемещение юнита и его возможное нападение или использование способности.
    /// </summary>
    /// <param name="ChosenUnit">Выбранный юнит.</param>
    /// <param name="board">Игровое поле.</param>
    /// <param name="userInput">Координаты текущей клетки юнита.</param>
    /// <param name="teamUnits">Словарь юнитов команды с их координатами.</param>
    /// <returns>True, если действия продолжаются; False, если завершены.</returns>
    private static bool HandleUnitMoveAndAttack(IUnit ChosenUnit, IGameBoard board, (int x, int y) userInput, Dictionary<(int x, int y), IUnit> teamUnits)
    {
        Scanner scanner = new(ChosenUnit.Speed);
        IEnumerable<ICell> availableMoves = scanner.Scan(board[userInput.x, userInput.y], board);
        GameBoardRenderer.DrawBoard(board, board[userInput.x, userInput.y], ChosenUnit.Speed);
        Console.WriteLine("Доступные ходы: ");
        foreach (var i in availableMoves) Console.Write($"({i.Position.X};{i.Position.Y}) | ");
        Console.WriteLine("\nПередвиньте юнита...");

        Console.Write("X = ");
        int newX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int newY = Convert.ToInt32(Console.ReadLine());
        Cell move = new((newX, newY));
        bool isLegal = availableMoves.Any(c => c.Position == move.Position);

        if (newX < 0 || newY < 0 || newX >= board.Width || newY >= board.Height || board[newX, newY].Content != null || !isLegal)
        {
            Console.WriteLine("Неверное перемещение");
            return true;
        }

        teamUnits[(newX, newY)] = ChosenUnit;
        board[userInput.x, userInput.y].Content = null;
        board[newX, newY].Content = ChosenUnit;
        teamUnits.Remove(userInput);
        GameBoardRenderer.DrawBoard(board);

        return HandleAttackOrAbility(ChosenUnit, board, userInput, newX, newY);
    }

    /// <summary>
    /// Обрабатывает нападение или использование способности выбранного юнита.
    /// </summary>
    /// <param name="ChosenUnit">Выбранный юнит.</param>
    /// <param name="board">Игровое поле.</param>
    /// <param name="userInput">Координаты текущей клетки юнита.</param>
    /// <param name="newX">Новые координаты X юнита.</param>
    /// <param name="newY">Новые координаты Y юнита.</param>
    /// <returns>True, если действия продолжаются; False, если завершены.</returns>
    private static bool HandleAttackOrAbility(IUnit ChosenUnit, IGameBoard board, (int x, int y) userInput, int newX, int newY)
    {
        Scanner scanner = new(ChosenUnit.Range);
        Console.WriteLine($"Дальность юнита: {ChosenUnit.Range}");
        Console.WriteLine($"{userInput.x}, {userInput.y}");
        Console.WriteLine($"{newX}, {newY}");
        IEnumerable<ICell> availableEnemies = scanner.Scan(board[newX, newY], board);
        Dictionary<(int x, int y), IUnit> enemyUnits = new();
        foreach (var i in availableEnemies)
        {
            if (i.Content is IUnit unit && unit.Team != ChosenUnit.Team)
                enemyUnits[i.Position] = unit;
        }

        if (enemyUnits.Count == 0)
        {
            Console.WriteLine("Нет врагов для атаки");
            ChosenUnit.MadeTurn = true;
            return false;
        }

        GameBoardRenderer.DrawBoard(board, board[newX, newY], ChosenUnit.Range);
        Console.WriteLine("Должен ли юнит атаковать? (1 - Да, 2 - Использовать способность, 3 - Пропустить ход): ");
        int userChoice = Convert.ToInt32(Console.ReadLine());

        switch (userChoice)
        {
            case 1:
                return HandleAttack(board, ChosenUnit, enemyUnits);
            case 2:
                return HandleAbility(board, ChosenUnit, enemyUnits);
            case 3:
                Console.WriteLine("Юнит остался на месте.");
                ChosenUnit.MadeTurn = true;
                return false;
            default:
                Console.WriteLine("Неверный выбор");
                return true;
        }
    }

    /// <summary>
    /// Обрабатывает атаку выбранного юнита на врага.
    /// </summary>
    /// <param name="board">Игровое поле.</param>
    /// <param name="ChosenUnit">Выбранный юнит.</param>
    /// <param name="enemyUnits">Словарь врагов с их координатами.</param>
    /// <returns>True, если действия продолжаются; False, если завершены.</returns>
    private static bool HandleAttack(IGameBoard board, IUnit ChosenUnit, Dictionary<(int x, int y), IUnit> enemyUnits)
    {
        Console.WriteLine("Враги, доступные для атаки:");
        foreach (var j in enemyUnits)
            Console.WriteLine($"{j.Value.UnitName} ({j.Key.x};{j.Key.y})");

        Console.WriteLine("Выберите врага. Введите координаты.");
        Console.Write("X = ");
        int enemyX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int enemyY = Convert.ToInt32(Console.ReadLine());
        (int enemyX, int enemyY) attackedEnemy = (enemyX, enemyY);

        if (!enemyUnits.ContainsKey(attackedEnemy))
        {
            Console.WriteLine("Неверное перемещение");
            return true;
        }

        Console.WriteLine("Атака на врага...");
        if (board[enemyX, enemyY].Content is IUnit target && target.Team != ChosenUnit.Team)
        {
            ChosenUnit.Attack(target);
            Console.WriteLine($"HP {target.UnitName} уменьшено до {target.Hp}");
            ChosenUnit.MadeTurn = true;
        }
        return false;
    }

    /// <summary>
    /// Обрабатывает использование способности выбранным юнитом.
    /// </summary>
    /// <param name="board">Игровое поле.</param>
    /// <param name="ChosenUnit">Выбранный юнит.</param>
    /// <param name="enemyUnits">Словарь врагов с их координатами.</param>
    /// <returns>True, если действия продолжаются; False, если завершены.</returns>
    private static bool HandleAbility(IGameBoard board, IUnit ChosenUnit, Dictionary<(int x, int y), IUnit> enemyUnits)
    {
        List<IAbility> activeAbilities = ChosenUnit.Abilities.FindAll(x => x.IsActive);
        if (activeAbilities.Count == 0)
        {
            Console.WriteLine("У юнита нет активных способностей.");
            return false;
        }

        int index = 1;
        foreach (var k in activeAbilities)
        {
            Console.WriteLine($"{index}. {k.Name} - {k.Description}");
            ++index;
        }
        Console.Write("Выберите способность ==> ");
        int chosenAbility = Convert.ToInt32(Console.ReadLine());
        if (chosenAbility < 1 || chosenAbility > activeAbilities.Count)
        {
            Console.WriteLine("Неверный выбор");
            return true;
        }

        IAbility Ability = activeAbilities[chosenAbility - 1];
        Console.WriteLine("Враги, доступные для применения способности:");
        foreach (var j in enemyUnits)
            Console.WriteLine($"{j.Value.UnitName} ({j.Key.x};{j.Key.y})");

        Console.WriteLine("Выберите врага. Введите координаты.");
        Console.Write("X = ");
        int enemyX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Y = ");
        int enemyY = Convert.ToInt32(Console.ReadLine());
        (int enemyX, int enemyY) attackedEnemy = (enemyX, enemyY);

        if (!enemyUnits.ContainsKey(attackedEnemy))
        {
            Console.WriteLine("Неверное перемещение");
            return true;
        }

        Console.WriteLine("Применение способности...");
        if (board[enemyX, enemyY].Content is IUnit target && target.Team != ChosenUnit.Team)
        {
            if (Ability.Activate(ChosenUnit, target))
            {
                Console.WriteLine($"HP {target.UnitName} уменьшено до {target.Hp}");
                ChosenUnit.MadeTurn = true;
            }
        }
        return false;
    }
}
