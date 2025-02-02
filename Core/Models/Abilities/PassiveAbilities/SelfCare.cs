namespace BoH.Models;

using BoH.Interfaces;
public class SelfCare : IAbility
{
    /// <inheritdoc/> 
    public string AbilityId { get; }

    /// <inheritdoc/> 
    public string Name { get; } = "Самолечение";

    /// <inheritdoc/> 
    public string Description { get; } = "В конце хода юнит лечит себя на 1-6 здоровья";

    /// <inheritdoc/> 
    public bool IsActive { get; } = false;

    /// <inheritdoc/> 
    public bool Activate(IUnit user, IUnit? target = null)
    {
        Random rnd = new Random();
        target = user;
        user.Heal(rnd.Next(1, 7));
        return true;
    }

    /// <inheritdoc/> 
    public void Update()
    {

    }

    public SelfCare()
    {
        AbilityId = Guid.NewGuid().ToString();
    }

}