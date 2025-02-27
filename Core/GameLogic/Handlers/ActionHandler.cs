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

        ICell originalCell = movingUnit.OccupiedCell;
        (int x, int y) originalPosition = originalCell.Position;
        _gameBoard[originalPosition.x, originalPosition.y].Content = null;
        originalCell.UpdateIcon();

        movingUnit.OccupiedCell = destination;
        destination.Content = movingUnit as IIconHolder;
        destination.UpdateIcon();

        movingUnit.ChangeTurnPhase();
        OnUpdatingGameBoard?.Invoke(_gameBoard);
    }

    /*
    /// <inheritdoc/>
    public void HandleAttack(IUnit attacker, ICell targetedCell, List<ICell> legalAttackLocations)
    {
        if (!legalAttackLocations.Contains(targetedCell)) throw new InvalidOperationException("Клетка не находится в радиусе атаки.");
        if (targetedCell.Content is IObstacle) throw new InvalidOperationException("Нельзя атаковать препятствие.");
        if (targetedCell.Content is null) throw new InvalidOperationException("Выбрана пустая клетка.");

        if (targetedCell.Content is IUnit target)
        {
            attacker.Attack(target);
            OnUpdatingGameBoard?.Invoke(_gameBoard);
        }
        else throw new InvalidDataException("Неизвестный тип объекта в клетке.");
    }
    */

    /// <inheritdoc/>
    public void HandleAction(IUnit attacker, IAbility usedAbility, List<ICell> legalAttackLocations, ICell? targetedCell = null)
    {
        if (targetedCell != null && !legalAttackLocations.Contains(targetedCell)) throw new InvalidOperationException("Клетка не находится в радиусе атаки.");
        if (!attacker.Abilities.Contains(usedAbility)) throw new InvalidOperationException("Способность отсутствует у юнита.");
        // if (targetedCell.Content is null) throw new InvalidOperationException("Выбрана пустая клетка.");
        // if (targetedCell!.Content is IObstacle obstacle) throw new InvalidOperationException("Нельзя атаковать препятствие.");
        if (targetedCell is null || targetedCell.Content is null)
        {
            usedAbility.Activate(attacker);
            attacker.ChangeTurnPhase();
            OnUpdatingGameBoard?.Invoke(_gameBoard);

        }
        else if (targetedCell!.Content is IUnit attacked)
        {
            usedAbility.Activate(attacked);
            attacker.ChangeTurnPhase();
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