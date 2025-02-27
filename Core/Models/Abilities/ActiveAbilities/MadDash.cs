namespace BoH.Models;

using BoH.Interfaces;

public class MadDash : IAbility
{
    /// <summary> Применяющий способность юнит. </summary>
    private readonly IUnit _abilityUser;

    /// <inheritdoc/> 
    public string AbilityId { get; }

    /// <inheritdoc/> 
    public string Name { get; } = "Безумный рывок";

    /// <inheritdoc/> 
    public string Description { get; } = "Вернитесь на фазу передвижения. Потеряйте 5 здоровья (не применится, если у применяющего юнита меньше 6 здоровья).";

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

        else if(_abilityUser.Hp < 6)
        {
            OnAbilityFailed?.Invoke(this);
            return false;
        }
        else
        {
            OnAbilityUsed?.Invoke(this);
            _abilityUser.CurrentTurnPhase = TurnPhase.End;
            _abilityUser.Hp -= 5;
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

    public MadDash(IUnit abilityUser)
    {
        _abilityUser = abilityUser;
        AbilityRange = _abilityUser.Speed;
        AbilityId = Guid.NewGuid().ToString();
    }

}