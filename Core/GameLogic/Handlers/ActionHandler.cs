namespace BoH.GameLogic;

using BoH.Interfaces;

/// <summary>
/// Реализация обработчика действий юнитов.
/// </summary>
public class ActionHandler : IActionHandler
{
    private IGameBoard _gameBoard;

    /// <inheritdoc/>
    public event Action<IGameBoard>? OnUpdatingGameBoard;

    /// <summary>
    /// Создает экземпляр обработчика действий.
    /// </summary>
    /// <param name="gameBoard">Игровое поле.</param>
    public ActionHandler(IGameBoard gameBoard)
    {
        _gameBoard = gameBoard;
    }

    /// <inheritdoc/>
    public void HandleMovement(IUnit movingUnit, ICell destination, List<ICell> legalMoves)
    {
        if (movingUnit.OccupiedCell == null) throw new ArgumentNullException("Юнит не был поставлен на поле.");
        if (legalMoves.Count == 0) throw new ArgumentException("Список доступных координат пуст.");
        if (!legalMoves.Any(cell => cell.Position.X == destination.Position.X && cell.Position.Y == destination.Position.Y))
            throw new InvalidOperationException("Клетка не находится в радиусе передвижения.");

        if (destination.IsOccupied()) throw new InvalidOperationException("Клетка занята, передвижение невозможно.");
        
        movingUnit.PlaceUnit(destination);
        OnUpdatingGameBoard?.Invoke(_gameBoard);
    }
    
    /// <inheritdoc/>
    public void HandleAction(IUnit attacker, IAbility usedAbility, List<ICell> legalAttackLocations, ICell? targetedCell = null)
    {
        if (targetedCell != null && !legalAttackLocations.Contains(targetedCell)) throw new InvalidOperationException("Клетка не находится в радиусе атаки.");
        if (!attacker.Abilities.Contains(usedAbility)) throw new InvalidOperationException("Способность отсутствует у юнита.");
        // if (targetedCell.Content is null) throw new InvalidOperationException("Выбрана пустая клетка.");
        // if (targetedCell!.Content is IObstacle obstacle) throw new InvalidOperationException("Нельзя атаковать препятствие.");
        if (targetedCell is null || targetedCell.Content is null)
        {
            usedAbility.Activate();
            OnUpdatingGameBoard?.Invoke(_gameBoard);
        }
        else if (targetedCell!.Content is IUnit attacked)
        {
            usedAbility.Activate(attacked);
            OnUpdatingGameBoard?.Invoke(_gameBoard);
        }
        else throw new InvalidDataException("Неизвестный тип объекта в клетке.");
    }

    /// <inheritdoc/>
    public void HandleSkip(IUnit unit)
    {
        if (unit.CurrentTurnPhase != TurnPhase.End) unit.ChangeTurnPhase();
        OnUpdatingGameBoard?.Invoke(_gameBoard);
    }
}