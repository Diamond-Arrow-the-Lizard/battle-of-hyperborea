namespace BoH.Models;

using BoH.Interfaces;

public class RusArcher : BaseUnit
{
    protected override int MaxHealth { get; } = 15;

    public RusArcher() : base("Рус-Лучник", 'Я', "Rus", UnitType.Range, 5)
    {
        Abilities.Add(new SelfCare(this));
        Hp = MaxHealth;
        Defence = 2;
        DamageDices = 3;
    }

}