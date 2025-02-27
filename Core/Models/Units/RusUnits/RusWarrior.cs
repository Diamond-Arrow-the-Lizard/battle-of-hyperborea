namespace BoH.Models;

public class RusWarrior : BaseUnit
{
    protected override int MaxHealth { get; } = 25;
    public RusWarrior() : base("Рус-Боец", 'R', "Rus")
    {
        Abilities.Add(new StunningBlow(this));
        Hp = MaxHealth;
        Defence = 5;
        DamageDices = 3;
    }

}