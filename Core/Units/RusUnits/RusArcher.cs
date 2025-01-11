namespace BoH.Units;

using BoH.Abilities;
using BoH.Interfaces;

public class RusArcher : BaseUnit
{
    protected override int MaxHealth { get; } = 10;
    public int AttackRange { get; set; } = 5;

    public RusArcher() : base("Рус-Лучник", 'Я', "Rus", UnitType.Range)
    {
        Abilities.Add(new SelfCare());
        Hp = MaxHealth;
        Defence = 2;
        DamageDices = 3;
    }

}