namespace BoH.Abilities;

using System.Security.Cryptography.X509Certificates;
using BoH.Interfaces;

public class MadDash : IAbility
{
    /// <inheritdoc/> 
    public string AbilityId { get; }

    /// <inheritdoc/> 
    public string Name { get; } = "Безумный рывок";

    /// <inheritdoc/> 
    public string Description { get; } = "Атакуйте выбранного юнита, не изменяя своего исходного движения. Потеряйте 5 здоровья (не применится, если у применяющего юнита меньше 6 здоровья).";

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
        else if(user.Hp < 6)
        {
            return false;
        }
        else
        {
            user.Attack(target);
            user.Hp -= 5;
            Coolown = 3;
            return true;
        }
    }

    /// <inheritdoc/> 
    public void Update()
    {
        if (Coolown != 0)
            --Coolown;
    }

    public MadDash()
    {
        AbilityId = Guid.NewGuid().ToString();
    }

}