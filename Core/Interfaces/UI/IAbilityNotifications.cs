namespace BoH.Interfaces;

public interface IAbilityNotifications
{
    void ShowAbilityInfo(IAbility ability);
    void Notify_AbilityUsed(IAbility ability);
    void Notify_AbilityOnCooldown(IAbility ability);
}