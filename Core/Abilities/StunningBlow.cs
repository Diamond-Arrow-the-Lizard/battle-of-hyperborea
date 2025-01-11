namespace BoH.Abilities;

using BoH.Interfaces;
using BoH.Models;

public class StunningBlow : IAbility
{
    
    /// <inheritdoc/> 
    public string AbilityId { get; }

    /// <inheritdoc/> 
    public string Name { get; } = "Ошеломляющий удар";

    /// <inheritdoc/> 
    public string Description { get; } = "Оглушает противника при ударе. Бросается только один кубик";

    /// <inheritdoc/> 
    public bool IsActive { get; } = true;

    /// <inheritdoc/> 
    public int Coolown { set; get; } = 0;

    /// <inheritdoc/> 
    public bool Activate(IUnit user, IUnit? target = null)
    {
        if (Coolown != 0)
            return false;

        if (target == null)
        {
            return false;
        }
        else
        {
            target.IsStunned = true;
            var userDices = user.DamageDices;
            user.DamageDices = 1;
            user.Attack(target);
            user.DamageDices = userDices;
            Coolown = 2;
            return true;
        }
    }

    /// <inheritdoc/> 
    public void Update()
    {
        if (Coolown != 0)
            --Coolown;
    }

    public StunningBlow()
    {
        AbilityId = Guid.NewGuid().ToString();
    }
}