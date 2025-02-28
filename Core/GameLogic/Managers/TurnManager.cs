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
    private IPlayer _currentPlayer;
    private readonly IPlayer[] _players;
    private readonly IActionHandler _actionHandler;
    private readonly IScannerHandler _scannerHandler;
    private int _currentPlayerIndex = 0;
    private readonly List<ICell> _availableUnitsCells = [];
    private TurnPhase? _unitTurnPhase;


    public IPlayer CurrentPlayer
    {
        get => _currentPlayer;
        set => _currentPlayer = value;
    }

    public IUnit? SelectedUnit { get; set; } = null;

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
        IPlayer[] players,
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

        SelectedUnit = unitCell.Content as IUnit ??
            throw new ArgumentNullException("В клетке не было юнита.");
        SelectedUnit.OccupiedCell = unitCell;
        _unitTurnPhase = SelectedUnit.CurrentTurnPhase;

        OnUnitSelected?.Invoke(SelectedUnit);

    }

    /// <inheritdoc/>
    /// <remarks> Для фазы движения или после неё не передавайте в метод параметры </remarks>
    public List<ICell> ProcessScanner(IAbility? usedAbility = null)
    {
        ArgumentNullException.ThrowIfNull(SelectedUnit);
        ArgumentNullException.ThrowIfNull(SelectedUnit.OccupiedCell);

        ICell scanningCell = SelectedUnit.OccupiedCell;
        List<ICell> scannedCells = new();

        switch (_unitTurnPhase)
        {
            case TurnPhase.Movement:
                scannedCells = _scannerHandler.HandleScan(scanningCell, SelectedUnit.Speed);
                break;
            case TurnPhase.Action:
                if (usedAbility != null)
                {
                    scannedCells = _scannerHandler.HandleScan(scanningCell, usedAbility.AbilityRange);
                }
                else
                {
                    scannedCells = _scannerHandler.HandleScan(scanningCell, SelectedUnit.Range);
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
        if (SelectedUnit == null) return;
        ArgumentNullException.ThrowIfNull(SelectedUnit.OccupiedCell);


        try
        {

            if (availableCells == null && target == null && usedAbility == null)
                SelectedUnit.CurrentTurnPhase = TurnPhase.End;
            _unitTurnPhase = SelectedUnit.CurrentTurnPhase;

            switch (_unitTurnPhase)
            {
                case TurnPhase.Movement:
                    ArgumentNullException.ThrowIfNull(availableCells);
                    Console.WriteLine(availableCells.Count);
                    if (target is ICell destination)
                    {
                        _availableUnitsCells.Remove(SelectedUnit.OccupiedCell);
                        _actionHandler.HandleMovement(SelectedUnit, destination, availableCells);
                        _availableUnitsCells.Add(SelectedUnit.OccupiedCell);
                        OnTurnStateChanged?.Invoke(SelectedUnit);
                    }
                    else throw new InvalidDataException("Передвижение осуществляется не на клетку.");

                    _unitTurnPhase = SelectedUnit.CurrentTurnPhase;
                    break;
                case TurnPhase.Action:
                    ArgumentNullException.ThrowIfNull(availableCells);
                    ArgumentNullException.ThrowIfNull(usedAbility);
                    if (target is ICell targetedCellForAbility)
                    {
                        _actionHandler.HandleAction(SelectedUnit, usedAbility, availableCells, targetedCellForAbility);
                        OnTurnStateChanged?.Invoke(SelectedUnit);
                    }
                    else if (target is null)
                    {
                        _actionHandler.HandleAction(SelectedUnit, usedAbility, availableCells, null);
                        OnTurnStateChanged?.Invoke(SelectedUnit);
                    }

                    _unitTurnPhase = SelectedUnit.CurrentTurnPhase;
                    break;
                case TurnPhase.End:
                    _actionHandler.HandleSkip(SelectedUnit);
                    OnTurnStateChanged?.Invoke(SelectedUnit);
                    break;
            }

            if (SelectedUnit is { CurrentTurnPhase: TurnPhase.End })
            {
                _availableUnitsCells.Remove(SelectedUnit.OccupiedCell);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"TurnManager's exception: {ex.Message}");
            if (!_availableUnitsCells.Contains(SelectedUnit.OccupiedCell))
            {
                _availableUnitsCells.Add(SelectedUnit.OccupiedCell); 
            }
        }
        finally
        {
            SelectedUnit = null;
        }
    }
}