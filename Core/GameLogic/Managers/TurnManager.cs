namespace BoH.GameLogic;

using BoH.Interfaces;
using BoH.Models;
using System.Linq;

/// <inheritdoc cref="ITurnManager"/>
public class TurnManager
{
    private Player _currentPlayer;
    private readonly Player[] _players;
    private readonly IActionHandler _actionHandler;
    private readonly IScannerHandler _scannerHandler;
    private int _currentPlayerIndex = 0;
    private List<IUnit> _availableUnits = new();
    private List<ICell> _availableUnitsCells = new();
    private IUnit? _selectedUnit;

    /// <summary>
    /// Инициализирует новый экземпляр менеджера ходов.
    /// </summary>
    /// <param name="players">Массив из двух игроков.</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если players не содержит ровно двух игроков.
    /// </exception>
    public TurnManager(Player[] players, IScanner scanner, IActionHandler actionHandler, IScannerHandler scannerHandler)
    {
        if (players.Length != 2)
            throw new ArgumentException("Требуется ровно два игрока", nameof(players));

        _players = players;
        _actionHandler = actionHandler;
        _scannerHandler = scannerHandler;
        _currentPlayer = _players[0];
    }

    /// <inheritdoc/>
    public event Action<IPlayer>? OnTurnEnd;

    /// <inheritdoc/>
    public event Action<IPlayer>? OnTurnStart;

    /// <inheritdoc/>
    public event Action<IUnit, AvailableActions>? OnUnitSelected;

    /// <inheritdoc/>
    public event Action<IUnit>? OnTurnStateChanged;

    /// <inheritdoc/>
    public void StartNewRound(IPlayer firstPlayer)
    {
        // Валидация и инициализация
        if (firstPlayer == null)
            throw new ArgumentNullException(nameof(firstPlayer), "Игрок не может быть null.");

        if (!_players.Contains(firstPlayer))
            throw new ArgumentException("Игрок не участвует в матче", nameof(firstPlayer));

        // Сброс состояний
        _currentPlayerIndex = Array.IndexOf(_players, firstPlayer);
        _currentPlayer = _players[_currentPlayerIndex];

        foreach (var player in _players)
            player.ResetUnitsForNewTurn();

        // Подготовка доступных юнитов
        _availableUnits = _currentPlayer.Units
            .Where(u => !u.IsDead && !u.IsStunned && u.Team == _currentPlayer.Team)
            .ToList();

        foreach(var i in _availableUnits)
        {
            Cell unitCell = new(i.Position);
            _availableUnitsCells.Add(unitCell);
        }

        OnTurnStart?.Invoke(_currentPlayer);
    }

    /// <inheritdoc/>
    public void EndTurn()
    {
        // Применение пассивных способностей
        foreach (var unit in _currentPlayer.Units)
        {
            foreach (var ability in unit.Abilities.Where(a => !a.IsActive))
                ability.Activate(unit);
        }

        // Переключение игрока
        OnTurnEnd?.Invoke(_currentPlayer);
        _availableUnitsCells.Clear();
        _availableUnits.Clear();
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
        _currentPlayer = _players[_currentPlayerIndex];
        _currentPlayer.ResetUnitsForNewTurn();
    }

    /// <inheritdoc/>
    public void SelectUnit(IUnit unit)
    {
        if (!_availableUnits.Contains(unit) || unit.IsDead)
            throw new InvalidOperationException("Юнит недоступен для выбора");

        _selectedUnit = unit;

        OnUnitSelected?.Invoke(unit, new AvailableActions
        {
            CanMove = unit.CurrentTurnPhase == TurnPhase.Movement && unit.CanMove(),
            CanAttack = unit.CurrentTurnPhase == TurnPhase.Action && unit.IsStunned == false && unit.IsDead == false,
            Abilities = unit.Abilities.Where(a => a.IsActive == true).ToList()
        });
    }

    /// <inheritdoc/>
    public List<ICell> ProcessScanner(ActionType action, int scanRange)
    {
        List<ICell> scannedCells = new();
        switch(action)
        {
            // TODO
        }

        return scannedCells;
    }

    /// <inheritdoc/>
    public void ProcessPlayerAction(ActionType action, List<ICell>? availableCells = null, object? target = null, IAbility? usedAbility = null)
    {
        if (_selectedUnit == null) return;

        try
        {
            switch (action)
            {
                case ActionType.Move:
                    ArgumentNullException.ThrowIfNull(availableCells);
                    if (target is ICell destination)
                    {
                        _actionHandler.HandleMovement(_selectedUnit, destination, availableCells);
                        OnTurnStateChanged?.Invoke(_selectedUnit);
                    } else throw new InvalidDataException("Передвижение осуществляется не на клетку.");
                    break;
                case ActionType.Attack:
                    ArgumentNullException.ThrowIfNull(availableCells);
                    if(target is ICell targetedCellForAttack)
                    {
                        _actionHandler.HandleAttack(_selectedUnit, targetedCellForAttack, availableCells);
                        OnTurnStateChanged?.Invoke(_selectedUnit);
                    } else throw new InvalidDataException("Атака осуществляется не на клетку.");
                    break;
                case ActionType.Ability:
                    ArgumentNullException.ThrowIfNull(availableCells);
                    ArgumentNullException.ThrowIfNull(usedAbility);
                    if (target is ICell targetedCellForAbility)
                    {
                        _actionHandler.HandleAbility(_selectedUnit, usedAbility, targetedCellForAbility, availableCells);
                        OnTurnStateChanged?.Invoke(_selectedUnit);
                    } else throw new InvalidDataException("Активация способности осуществляется не на клетку.");
                    break;
                case ActionType.Skip:
                    _actionHandler.HandleSkip(_selectedUnit);
                    OnTurnStateChanged?.Invoke(_selectedUnit);
                    break;
            }

            if (_selectedUnit.CurrentTurnPhase == TurnPhase.End)
                _availableUnits.Remove(_selectedUnit);

            OnTurnStateChanged?.Invoke(_selectedUnit);
        }
        finally
        {
            _selectedUnit = null;
        }
    }
}