namespace BoH.GameLogic;

using BoH.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Класс для управления игровым процессом.
/// </summary>
public class GameController : IGameController
{
    private readonly IGameBoardService _boardService;
    private IGameBoard? _gameBoard;
    private Dictionary<string, List<IUnit>> _teamUnits = new(); // Словарь юнитов по командам
    private int _currentTeamIndex = 0; // Индекс текущей команды

    public GameController(IGameBoardService boardService)
    {
        _boardService = boardService ?? throw new ArgumentNullException(nameof(boardService));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"/>
    public async Task StartGame(int width, int length, Dictionary<string, List<IUnit>> teamUnits)
    {
        if (_teamUnits.Keys.Count != 2)
        {
            throw new ArgumentException("Игра требует две команды.");
        }

        _teamUnits = teamUnits;
        _gameBoard = _boardService.GenerateGameBoard(width, length, _teamUnits);

        await _boardService.DeleteGameBoardAsync();
        await _boardService.SaveGameBoardAsync();

        Console.WriteLine("Игра началась!");
    }

    /// <inheritdoc/>
    public void NextTurn(string[] teams)
    {
        if (_gameBoard == null || !_teamUnits.Any())
        {
            throw new InvalidOperationException("Игра не была инициализирована.");
        }

        // Получаем текущую команду
        var currentTeam = teams[_currentTeamIndex];
        Console.WriteLine($"Ход команды: {currentTeam}");

        // Получаем юнитов команды, которые могут совершать действия
        List<IUnit> activeUnits = _teamUnits[currentTeam].Where(unit => !unit.IsDead && !unit.IsStunned).ToList();

        foreach (var unit in activeUnits)
        {
            //TODO
        }

        // Проверяем победу
        if (CheckVictoryCondition())
        {
            Console.WriteLine($"Команда {currentTeam} победила!");
            return;
        }

        // Переходим к следующей команде
        _currentTeamIndex = (_currentTeamIndex + 1) % teams.Length;
    }

    /// <inheritdoc/>
    public bool CheckVictoryCondition()
    {
        // Проверяем, осталась ли только одна команда с юнитами
        var survivingTeams = _teamUnits
            .Where(kvp => kvp.Value.Any(unit => !unit.IsDead))
            .Select(kvp => kvp.Key)
            .ToList();

        return survivingTeams.Count == 1;
    }

    /// <inheritdoc/>
    public async Task EndGame()
    {
        _gameBoard = null;
        _teamUnits.Clear();
        await _boardService.DeleteGameBoardAsync();

        Console.WriteLine("Игра завершена!");
    }

}
