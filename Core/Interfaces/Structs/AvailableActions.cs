namespace BoH.Interfaces;
public struct AvailableActions
{
    public bool CanMove;
    public bool CanAttack;
    public List<IAbility> Abilities;
}