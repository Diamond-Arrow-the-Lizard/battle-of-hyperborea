namespace BoH.Units;

using BoH.Abilities;

public class RusWarrior : BaseUnit
{
    private int _maxHealth = 20;

    public RusWarrior() : base('R', "Rus")
    {
        Abilities.Add(new StunningBlow());
        Hp = _maxHealth;
        Defence = 8;
        DamageDices = 2;
    }

}