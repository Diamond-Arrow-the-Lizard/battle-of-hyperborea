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
        if (!legalMoves.Contains(destination)) throw new InvalidOperationException("Клетка не находится в радиусе передвижения.");
        if (destination.IsOccupied()) throw new InvalidOperationException("Клетка занята, передвижение невозможно.");
        
        (int x, int y) originalPosition = movingUnit.OccupiedCell.Position;
        _gameBoard[originalPosition.x, originalPosition.y].Content = null;
        
        movingUnit.OccupiedCell = destination;
        destination.Content = movingUnit as IIconHolder;
        
        movingUnit.ChangeTurnPhase();
        OnUpdatingGameBoard?.Invoke(_gameBoard);
    }
    
    /// <inheritdoc/>
    public void HandleAttack(IUnit attacker, ICell targetedCell, List<ICell> legalAttackLocations)
    {
        if (!legalAttackLocations.Contains(targetedCell)) throw new InvalidOperationException("Клетка не находится в радиусе атаки.");
        if (targetedCell.Content is IObstacle) throw new InvalidOperationException("Нельзя атаковать препятствие.");
        if (targetedCell.Content is null) throw new InvalidOperationException("Выбрана пустая клетка.");

        if (targetedCell.Content is IUnit target)
        {
            attacker.Attack(target);
            attacker.ChangeTurnPhase();
            OnUpdatingGameBoard?.Invoke(_gameBoard);
        }
        else throw new InvalidDataException("Неизвестный тип объекта в клетке.");
    }
    
    /// <inheritdoc/>
    public void HandleAbility(IUnit attacker, IAbility usedAbility, ICell targetedCell, List<ICell> legalAttackLocations)
    {
        if (!legalAttackLocations.Contains(targetedCell)) throw new InvalidOperationException("Клетка не находится в радиусе атаки.");
        if (!attacker.Abilities.Contains(usedAbility)) throw new InvalidOperationException("Способность отсутствует у юнита.");
        if (targetedCell.Content is IObstacle) throw new InvalidOperationException("Нельзя атаковать препятствие.");
        if (targetedCell.Content is null) throw new InvalidOperationException("Выбрана пустая клетка.");

        if (targetedCell.Content is IUnit target)
        {
            usedAbility.Activate(attacker, target);
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