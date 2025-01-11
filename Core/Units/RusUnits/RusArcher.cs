namespace BoH.Units;

using BoH.Abilities;
using BoH.Interfaces;

public class RusArcher : BaseUnit
{
    private int _maxHealth = 10;

    public int AttackRange {get; set;} = 5;

    public RusArcher() : base('Я', "Rus", UnitType.Range)
    {
        Abilities.Add(new SelfCare());
        Hp = _maxHealth;
        Defence = 2;
        DamageDices = 3;
    }

}