
namespace BoH.Models;

using BoH.Interfaces;

public class LizardArcher : BaseUnit
{
    protected override int MaxHealth { get; } = 12;
    public int AttackRange { get; set; } = 4;

    public LizardArcher() : base("Ящер-Лучник", '2', "Lizard", UnitType.Range)
    {
        Abilities.Add(new SelfCare(this));
        Hp = MaxHealth;
        Defence = 2;
        DamageDices = 3;
    }

}