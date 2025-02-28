namespace BoH.Interfaces;

public interface IAbilityNotifications
{
    void Notify_AbilityUsed(IAbility ability);
    void Notify_AbilityOnCooldown(IAbility ability);
    void Notify_AbilityFailed(IAbility ability);
}