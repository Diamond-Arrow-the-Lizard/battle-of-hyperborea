namespace BoH.GameLogic;

using BoH.Interfaces;
using BoH.Models;
using System.Linq;

/// <inheritdoc cref="ITurnManager"/>
/// <remarks>
/// Управляет очередью ходов, переключением между игроками и обработкой игровых действий.
/// Взаимодействует с <see cref="IScannerHandler"/> для определения доступных клеток
/// и с <see cref="IActionHandler"/> для выполнения действий.
/// </remarks>
public class TurnManager : ITurnManager
{
    private readonly IGameBoard _gameBoard;
    private Player _currentPlayer;
    private readonly Player[] _players;
    private readonly IActionHandler _actionHandler;
    private readonly IScannerHandler _scannerHandler;
    private int _currentPlayerIndex = 0;
    private readonly List<ICell> _availableUnitsCells = [];
    private IUnit? _selectedUnit;
    private TurnPhase? _unitTurnPhase;


    public IPlayer CurrentPlayer
    {
        get => _currentPlayer;
        set => _currentPlayer = value as Player ?? throw new ArgumentNullException("Player is null");
    }

    public IUnit? SelectedUnit
    {
        get { return _selectedUnit; }
        set { _selectedUnit = value; }
    }

    /// <inheritdoc/>
    public event Action<IPlayer>? OnTurnEnd;

    /// <inheritdoc/>
    public event Action<IPlayer>? OnTurnStart;

    /// <inheritdoc/>
    public event Action<IUnit>? OnUnitSelected;

    /// <inheritdoc/>
    public event Action<IUnit>? OnTurnStateChanged;


    /// <summary>
    /// Инициализирует новый экземпляр менеджера ходов.
    /// </summary>
    /// <param name="gameBoard">Игровое поле, на котором происходит действие.</param>
    /// <param name="players">Массив из двух игроков.</param>
    /// <param name="scanner">Сканер для определения доступных клеток.</param>
    /// <param name="actionHandler">Обработчик игровых действий.</param>
    /// <param name="scannerHandler">Обработчик сканирования клеток.</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="players"/> не содержит ровно двух игроков.
    /// </exception>
    public TurnManager(
        IGameBoard gameBoard,
        Player[] players,
        IActionHandler actionHandler,
        IScannerHandler scannerHandler)
    {
        if (players.Length != 2)
            throw new ArgumentException("Требуется ровно два игрока", nameof(players));

        _gameBoard = gameBoard;
        _players = players;
        _actionHandler = actionHandler;
        _scannerHandler = scannerHandler;
        _currentPlayer = _players[0];
    }

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
        for (int x = 0; x < _gameBoard.Width; x++)
        {
            for (int y = 0; y < _gameBoard.Height; y++)
            {
                if (_gameBoard[x, y].Content is IUnit unit &&
                    unit.Team == _currentPlayer.Team &&
                    unit.CanMove())
                {
                    _availableUnitsCells.Add(_gameBoard[x, y]);
                }
            }
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
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
        _currentPlayer = _players[_currentPlayerIndex];
        _currentPlayer.ResetUnitsForNewTurn();
    }

    /// <inheritdoc/>
    public void SelectUnit(ICell unitCell)
    {
        if (!_availableUnitsCells.Contains(unitCell))
            throw new InvalidOperationException("Юнит недоступен для выбора.");

        _selectedUnit = unitCell.Content as IUnit ??
            throw new ArgumentNullException("В клетке не было юнита.");
        _selectedUnit.OccupiedCell = unitCell;
        _unitTurnPhase = _selectedUnit.CurrentTurnPhase;

        OnUnitSelected?.Invoke(_selectedUnit);

    }

    /// <inheritdoc/>
    /// <remarks> Для фазы движения или после неё не передавайте в метод параметры </remarks>
    public List<ICell> ProcessScanner(IAbility? usedAbility = null)
    {
        ArgumentNullException.ThrowIfNull(_selectedUnit);
        ArgumentNullException.ThrowIfNull(_selectedUnit.OccupiedCell);

        ICell scanningCell = _selectedUnit.OccupiedCell;
        List<ICell> scannedCells = new();

        switch (_unitTurnPhase)
        {
            case TurnPhase.Movement:
                scannedCells = _scannerHandler.HandleScan(scanningCell, _selectedUnit.Speed);
                break;
            case TurnPhase.Action:
                if (usedAbility != null)
                {
                    scannedCells = _scannerHandler.HandleScan(scanningCell, usedAbility.AbilityRange);
                }
                else
                {
                    scannedCells = _scannerHandler.HandleScan(scanningCell, _selectedUnit.Range);
                }
                break;
        }

        return scannedCells;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Для пропуска хода все параметры должны быть null (либо ничего не передавать, либо ставить null, null null).
    /// </remarks>
    public void ProcessPlayerAction(
        List<ICell>? availableCells = null,
        object? target = null,
        IAbility? usedAbility = null)
    {
        if (_selectedUnit == null) return;
        ArgumentNullException.ThrowIfNull(_selectedUnit.OccupiedCell);


        try
        {

            if (availableCells == null && target == null && usedAbility == null) _selectedUnit.CurrentTurnPhase = TurnPhase.End;
            _unitTurnPhase = _selectedUnit.CurrentTurnPhase;

            switch (_unitTurnPhase)
            {
                case TurnPhase.Movement:
                    ArgumentNullException.ThrowIfNull(availableCells);
                    Console.WriteLine(availableCells.Count);
                    if (target is ICell destination)
                    {
                        _availableUnitsCells.Remove(_selectedUnit.OccupiedCell);
                        _actionHandler.HandleMovement(_selectedUnit, destination, availableCells);
                        _availableUnitsCells.Add(_selectedUnit.OccupiedCell);
                        OnTurnStateChanged?.Invoke(_selectedUnit);
                    }
                    else throw new InvalidDataException("Передвижение осуществляется не на клетку.");

                    _unitTurnPhase = _selectedUnit.CurrentTurnPhase;
                    break;
                case TurnPhase.Action:
                    ArgumentNullException.ThrowIfNull(availableCells);
                    ArgumentNullException.ThrowIfNull(usedAbility);
                    if (target is ICell targetedCellForAbility)
                    {
                        _actionHandler.HandleAction(_selectedUnit, usedAbility, availableCells, targetedCellForAbility);
                        OnTurnStateChanged?.Invoke(_selectedUnit);
                    }
                    else if (target is null)
                    {
                        _actionHandler.HandleAction(_selectedUnit, usedAbility, availableCells, null);
                        OnTurnStateChanged?.Invoke(_selectedUnit);
                    }
                    _unitTurnPhase = _selectedUnit.CurrentTurnPhase;
                    // else throw new InvalidDataException("Активация способности осуществляется не на клетку.");
                    break;
                case TurnPhase.End:
                    _actionHandler.HandleSkip(_selectedUnit);
                    OnTurnStateChanged?.Invoke(_selectedUnit);
                    break;
            }

            if (_selectedUnit.CurrentTurnPhase == TurnPhase.End)
                _availableUnitsCells.Remove(_selectedUnit.OccupiedCell);

            OnTurnStateChanged?.Invoke(_selectedUnit);
        }
        finally
        {
            _selectedUnit = null;
        }
    }
}