namespace BoH.GameLogic;

using BoH.Interfaces;
using BoH.Models;
using System.Linq;

/// <inheritdoc/>
public class TurnManager : ITurnManager
{
    private Player _currentPlayer;
    private readonly Player[] _players;
    private int _currentPlayerIndex = 0;
    public TurnManager(Player[] players)
    {
        if (players.Length != 2) throw new IndexOutOfRangeException("Игроков должно быть только двое.");
        else
        {
            _players = players;
            _currentPlayer = _players[0];
        }
    }

    /// <inheritdoc/>
    public event Action<IPlayer>? OnTurnEnd;

    /// <inheritdoc/>
    public event Action<IPlayer>? OnTurnStart;

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если firstPlayer равен null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если firstPlayer не является участником игры.
    /// </exception>
    public void StartNewRound(IPlayer firstPlayer)
    {
        // Валидация входных данных
        if (firstPlayer == null)
            throw new ArgumentNullException(nameof(firstPlayer), "Игрок не может быть null.");

        if (!_players.Contains(firstPlayer))
            throw new ArgumentException("Указанный игрок не участвует в текущей игре.", nameof(firstPlayer));

        // Инициализация состояния
        _currentPlayerIndex = Array.IndexOf(_players, firstPlayer);
        _currentPlayer = _players[_currentPlayerIndex];

        // Сброс состояний всех юнитов
        foreach (var player in _players)
        {
            player.ResetUnitsForNewTurn();
        }

        // Уведомление о начале хода
        OnTurnStart?.Invoke(_currentPlayer);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если не все юниты завершили фазы.
    /// </exception>
    public void EndTurn()
    {
        // Проверка завершения фаз
        if (_currentPlayer.Units.Any(u => u.CurrentTurnPhase != TurnPhase.End))
            throw new InvalidOperationException("Не все юниты завершили свои фазы.");

        // Применение пассивных способностей
        foreach (var unit in _currentPlayer.Units)
        {
            var passiveAbilities = unit.Abilities.Where(a => !a.IsActive);
            foreach (var ability in passiveAbilities)
            {
                ability.Activate(unit);
            }
        }

        // Уведомление о завершении хода
        OnTurnEnd?.Invoke(_currentPlayer);

        // Переключение игрока
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
        _currentPlayer = _players[_currentPlayerIndex];

        // Сброс фаз нового игрока
        _currentPlayer.ResetUnitsForNewTurn();
    }
}