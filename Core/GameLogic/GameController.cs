namespace BoH.GameLogic;

using BoH.Interfaces;
using BoH.Models;
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
    public IGameBoard StartGame(int width, int length, Dictionary<string, List<IUnit>> teamUnits)
    {
        _teamUnits = teamUnits;
        if (_teamUnits.Keys.Count != 2)
        {
            throw new ArgumentException("Игра требует две команды.");
        }

        _gameBoard = _boardService.GenerateGameBoard(width, length, _teamUnits);

        _boardService.DeleteGameBoardAsync();
        _boardService.SaveGameBoardAsync();

        return _gameBoard;

    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="InvalidOperationException"/>
    public int NextTurn()
    {
        if (_gameBoard == null || !_teamUnits.Any())
        {
            throw new InvalidOperationException("Игра не была инициализирована.");
        }

        // Получаем текущую команду
        List<string> teamNames = _teamUnits.Keys.ToList();
        string currentTeam = teamNames[_currentTeamIndex];

        // Сброс состояния MadeTurn для всех юнитов
        foreach (var unit in _teamUnits[currentTeam])
        {
            if (!unit.IsDead && !unit.IsStunned)
            {
                unit.MadeTurn = false;
            }
        }

        // Проверяем победу
        if (CheckVictoryCondition())
        {
            Console.WriteLine($"Команда {currentTeam} победила!");
            return 2;
        }

        // Переходим к следующей команде
        _currentTeamIndex = (_currentTeamIndex + 1) % teamNames.Count;

        return _currentTeamIndex;
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
