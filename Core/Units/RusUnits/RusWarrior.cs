namespace BoH.Units;

using BoH.Abilities;

public class RusWarrior : BaseUnit
{
    protected override int MaxHealth { get; } = 10;
    public RusWarrior() : base('R', "Rus")
    {
        Abilities.Add(new StunningBlow());
        Hp = MaxHealth;
        Defence = 8;
        DamageDices = 2;
    }

}