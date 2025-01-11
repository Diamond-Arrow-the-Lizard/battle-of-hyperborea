namespace BoH.Units;

using BoH.Abilities;

public class LizardWarrior : BaseUnit
{
    protected override int MaxHealth { get; } = 10;
    public LizardWarrior() : base("Ящер-Боец", 'S', "Lizard")
    {
        Abilities.Add(new MadDash());
        Hp = MaxHealth;
        Defence = 8;
        DamageDices = 2;
    }

}