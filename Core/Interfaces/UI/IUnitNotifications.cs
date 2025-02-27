namespace BoH.Interfaces;

public interface IUnitNotifications
{
    void ShowUnitStats(IUnit unit);
    void Notify_UnitDead(IUnit unit);
    void Notify_UnitStunned(IUnit unit);
    void Notify_UnitMoved(IUnit unit);
    void Notify_UnitAttacked(IUnit unit, int amount);
    void Notify_UnitRecievedDamage(IUnit unit, int amount);
    void Notify_UnitUsedAbility(IUnit unit);
    void Notify_UnitHealed(IUnit unit, int amount);
    void Notify_UnitChangedTurnPhase(IUnit unit);
}