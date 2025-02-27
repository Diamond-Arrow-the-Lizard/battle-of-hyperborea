namespace BoH.Interfaces;

/// <summary>
/// Интерфейс обработчика действий юнитов.
/// </summary>
public interface IActionHandler
{
    /// <summary>
    /// Событие, вызываемое при обновлении игрового поля.
    /// </summary>
    event Action<IGameBoard>? OnUpdatingGameBoard;

    /// <summary>
    /// Обрабатывает перемещение юнита на указанную клетку.
    /// </summary>
    /// <param name="movingUnit">Юнит, совершающий перемещение.</param>
    /// <param name="destination">Целевая клетка для перемещения.</param>
    /// <param name="legalMoves">Список доступных клеток для перемещения.</param>
    void HandleMovement(IUnit movingUnit, ICell destination, List<ICell> legalMoves);
/*
    /// <summary>
    /// Обрабатывает атаку юнита на указанную клетку.
    /// </summary>
    /// <param name="attacker">Атакующий юнит.</param>
    /// <param name="targetedCell">Целевая клетка для атаки.</param>
    /// <param name="legalAttackLocations">Список доступных клеток для атаки.</param>
    void HandleAttack(IUnit attacker, ICell targetedCell, List<ICell> legalAttackLocations);
*/
    /// <summary>
    /// Обрабатывает применение способности юнита.
    /// </summary>
    /// <param name="attacker">Юнит, использующий способность.</param>
    /// <param name="usedAbility">Используемая способность.</param>
    /// <param name="targetedCell">Целевая клетка для способности.</param>
    /// <param name="legalAttackLocations">Список доступных клеток для применения способности.</param>
    void HandleAction(IUnit attacker, IAbility usedAbility, List<ICell> legalAttackLocations, ICell? targetedCell);

    /// <summary>
    /// Обрабатывает пропуск хода юнитом.
    /// </summary>
    /// <param name="unit">Юнит, пропускающий ход.</param>
    void HandleSkip(IUnit unit);
}
