namespace BoH.Services;

using BoH.Interfaces;
using BoH.Models;
using System.Threading.Tasks;

/// <summary>
/// Сервис для управления игровым полем.
/// </summary>
public class GameBoardService : IGameBoardService
{
    /// <inheritdoc/>
    public IGameBoard GenerateGameBoard(int width, int height, IEnumerable<IUnit> units, ref IPlayer[] players)
    {
        // Проверка входных данных
        if (players == null || players.Length < 2)
            throw new ArgumentException("Массив игроков должен содержать минимум двух игроков.", nameof(players));

        Random rnd = new Random();
        var gameBoard = new GameBoard(width, height);

        // Сбор имен команд из всех юнитов
        List<string> teamNames = new List<string>();
        foreach (var unit in units)
            teamNames.Add(unit.Team); 

        var uniqueTeams = teamNames.Distinct().ToList();
        if (uniqueTeams.Count != 2)
            throw new InvalidOperationException("Число команд не равно двум.");

        // Распределение юнитов по игрокам в зависимости от команды
        foreach (var unit in units)
        {
            if (unit.Team == uniqueTeams[0])
                players[0].AddUnit(unit);
            else if (unit.Team == uniqueTeams[1])
                players[1].AddUnit(unit);
            else
                throw new InvalidOperationException($"Неизвестная команда: {unit.Team}");
        }

        // Проверка: число юнитов для каждого игрока не должно превышать допустимое число (например, не больше количества столбцов)
        if (players[0].Units.Count > gameBoard.Width || players[1].Units.Count > gameBoard.Width)
            throw new InvalidOperationException("Количество юнитов превышает допустимое число для игрового поля.");

        // Размещение юнитов в диаметрально противоположных углах:
        // Для наглядности, выберем следующую стратегию:
        // - Юниты первого игрока (команда uniqueTeams[0]) размещаются по верхней строке слева направо.
        // - Юниты второго игрока (команда uniqueTeams[1]) – по нижней строке справа налево.
        int pos = 0;
        foreach (var unit in players[0].Units)
        {
            if (unit is IIconHolder iconHolder)
            {
                // Верхняя строка (индекс 0), колонки слева направо
                gameBoard[pos, 0].Content = iconHolder;
                pos++;
            }
        }

        pos = 0;
        foreach (var unit in players[1].Units)
        {
            if (unit is IIconHolder iconHolder)
            {
                // Нижняя строка (индекс Height - 1), колонки справа налево
                gameBoard[gameBoard.Width - 1 - pos, gameBoard.Height - 1].Content = iconHolder;
                pos++;
            }
        }

        // Размещение препятствий. Количество препятствий случайное от 0 до gameBoard.Width - 1.
        int obstacleCount = rnd.Next(0, gameBoard.Width);
        int failedPlacements = 0;
        while (obstacleCount > 0)
        {
            int x = rnd.Next(0, gameBoard.Width);
            int y = rnd.Next(0, gameBoard.Height);
            if (gameBoard[x, y].Content != null)
            {
                failedPlacements++;
                if (failedPlacements == 3)
                    break;
            }
            else
            {
                gameBoard[x, y].Content = new Obstacle();
                obstacleCount--;
                // Сброс счётчика неудачных попыток после успешного размещения
                failedPlacements = 0;
            }
        }

        return gameBoard;
    }


    /// <inheritdoc/>
    public void AddObjectToGameBoard(IIconHolder? obj, ICell cell)
    {
        if (obj == null) return;
        cell.Content = obj;
    }

    /// <inheritdoc/>
    public void RemoveObjectFromGameBoard(IIconHolder? obj, ICell cell)
    {
        if (obj == null) return;
        cell.Content = null;
    }

    /// <inheritdoc/>
    public async Task SaveGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }

    /// <inheritdoc/>
    public async Task LoadGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }

    /// <inheritdoc/>
    public async Task DeleteGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }
}
