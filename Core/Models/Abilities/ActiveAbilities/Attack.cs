using BoH.Interfaces;

namespace BoH.Models;

public class Attack : IAbility
{
    /// <summary> Применяющий способность юнит. </summary>
    private readonly IUnit _abilityUser;

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
    public int AbilityRange { get; set; }

    /// <inheritdoc/> 
    public event Action<IAbility>? OnAbilityUsed;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnAbilityFailed;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnCooldown = null;

    /// <inheritdoc/> 
    public bool Activate(IUnit? target = null)
    {
        if (target == null) 
        {
            OnAbilityFailed?.Invoke(this);
            return false;
        }

        else if (Coolown > 0)
        {
            OnAbilityFailed?.Invoke(this);
            OnCooldown?.Invoke(this);
            return false;
        }

        else
        {
            OnAbilityUsed?.Invoke(this);
            _abilityUser.Attack(target);
            return true;
        }
    }

    /// <inheritdoc/> 
    public void Update() { return; }

    public Attack(IUnit abilityUser)
    {
        _abilityUser = abilityUser;
        AbilityRange = _abilityUser.Range;

        AbilityId = Guid.NewGuid().ToString();
        OnCooldown = null;
    }
}