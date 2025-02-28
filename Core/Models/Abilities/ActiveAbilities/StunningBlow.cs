namespace BoH.Models;

using BoH.Interfaces;

public class StunningBlow : IAbility
{

    /// <summary> Применяющий способность юнит. </summary>
    private readonly IUnit _abilityUser;
    
    /// <inheritdoc/> 
    public string AbilityId { get; }

    /// <inheritdoc/> 
    public string Name { get; } = "Ошеломляющий удар";

    /// <inheritdoc/> 
    public string Description { get; } = "Оглушает противника при ударе. Бросается только один кубик.";

    /// <inheritdoc/> 
    public bool IsActive { get; } = true;

    /// <inheritdoc/> 
    public int Coolown { set; get; } = 0;

    /// <inheritdoc/> 
    public int AbilityRange { set; get; }

    /// <inheritdoc/> 
    public event Action<IAbility>? OnAbilityUsed;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnAbilityFailed;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnCooldown;

    /// <inheritdoc/> 
    public bool Activate(IUnit? target = null)
    {
        if (Coolown != 0) {
            OnAbilityFailed?.Invoke(this);
            OnCooldown?.Invoke(this);
            return false;
        }

        if (target == null)
        {
            OnAbilityFailed?.Invoke(this);
            return false;
        }
        else
        {
            OnAbilityUsed?.Invoke(this);
            var userDices = _abilityUser.DamageDices;
            _abilityUser.DamageDices = 1;
            _abilityUser.Attack(target);
            _abilityUser.DamageDices = userDices;
            Coolown = 2;
            target.GetStunned();
            _abilityUser.ChangeTurnPhase();
            return true;
        }
    }

    /// <inheritdoc/> 
    public void Update()
    {
        if (Coolown != 0)
            --Coolown;
    }

    public StunningBlow(IUnit abilityUser)
    {
        _abilityUser = abilityUser;
        AbilityRange = _abilityUser.Range;
        AbilityId = Guid.NewGuid().ToString();
    }
}