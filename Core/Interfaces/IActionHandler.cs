namespace BoH.Interfaces;

public interface IActionHandler
{
    event Action<IGameBoard>? OnUpdatingGameBoard;

    void HandleMovement(IUnit movingUnit, ICell destination, List<ICell> legalMoves);
    void HandleAttack(IUnit attacker, ICell targetedCell, List<ICell> legalAttackLocations);
    void HandleAbility(IUnit attacker, IAbility usedAbility, ICell targetedCell, List<ICell> legalAttackLocations);
    void HandleSkip(IUnit unit);
}