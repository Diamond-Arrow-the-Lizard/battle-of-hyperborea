namespace BoH.Models;

public class LizardWarrior : BaseUnit
{
    protected override int MaxHealth { get; } = 20;
    public LizardWarrior() : base("Ящер-Боец", 'S', "Lizard")
    {
        Abilities.Add(new MadDash());
        Hp = MaxHealth;
        Defence = 4;
        DamageDices = 3;
    }

}