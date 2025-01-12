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
    private readonly Dictionary<string, List<IUnit>> _teamUnits = new(); // Словарь юнитов по командам
    private int _currentTeamIndex = 0; // Индекс текущей команды

    public GameController(IGameBoardService boardService)
    {
        _boardService = boardService ?? throw new ArgumentNullException(nameof(boardService));
    }

    /// <inheritdoc/>
    public async Task StartGame(IGameBoard gameBoard, string[] teams)
    {
        if (teams == null || teams.Length < 2)
        {
            throw new ArgumentException("Игра требует минимум две команды.");
        }

        _gameBoard = gameBoard ?? throw new ArgumentNullException(nameof(gameBoard));
        _teamUnits.Clear();

        //TODO Инициализация юнитов для каждой команды

        // Сохраняем начальное состояние игрового поля
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
