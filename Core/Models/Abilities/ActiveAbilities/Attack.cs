using BoH.Interfaces;

namespace BoH.Models;

public class Attack : IAbility
{
    /// <inheritdoc/> 
    public string AbilityId { get; }

    /// <inheritdoc/> 
    public string Name { get; } = "Атака";

    /// <inheritdoc/> 
    public string Description { get; } = "Атакуйте врага. Кол-во урона зависит от числа бросаемых кубиков";

    /// <inheritdoc/> 
    public bool IsActive { get; } = true;

    /// <inheritdoc/> 
    public int Coolown { set; get; } = 0;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnAbilityUsed;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnCooldown = null;

    /// <inheritdoc/> 
    public bool Activate(IUnit user, IUnit? target = null)
    {
        if (target == null) return false;

        else if (Coolown > 0)
        {
            OnCooldown?.Invoke(this);
            return false;
        }

        else
        {
            OnAbilityUsed?.Invoke(this);
            user.Attack(target);
            return true;
        }
    }

    /// <inheritdoc/> 
    public void Update()
    {
        throw new InvalidOperationException("Обычная атака не имеет кулдауна.");
    }

    public Attack()
    {
        AbilityId = Guid.NewGuid().ToString();
        OnCooldown = null;
    }
}