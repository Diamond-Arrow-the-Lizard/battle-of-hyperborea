namespace BoH.Models;

using BoH.Interfaces;
public class SelfCare : IAbility
{
    /// <summary> Применяющий способность юнит. </summary>
    private readonly IUnit _abilityUser;

    /// <inheritdoc/> 
    public string AbilityId { get; }

    /// <inheritdoc/> 
    public string Name { get; } = "Самолечение";

    /// <inheritdoc/> 
    public string Description { get; } = "В конце хода юнит лечит себя на 1-6 здоровья";

    /// <inheritdoc/> 
    public bool IsActive { get; } = false;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnAbilityUsed;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnAbilityFailed;

    /// <inheritdoc/> 
    public event Action<IAbility>? OnCooldown = null;

    /// <inheritdoc/> 
    public int Coolown { set; get; } = 0;

    /// <inheritdoc/> 
    public int AbilityRange { set; get; } = 0;

    /// <inheritdoc/> 
    public bool Activate(IUnit? target = null)
    {
        if (Coolown != 0)
        {
            OnAbilityFailed?.Invoke(this);
            OnCooldown?.Invoke(this);
            return false;
        }

        Random rnd = new Random();
        target ??= _abilityUser;
        _abilityUser.Heal(rnd.Next(1, 7));
        OnAbilityUsed?.Invoke(this);
        return true;
    }

    /// <inheritdoc/> 
    public void Update() { return; }

    public SelfCare(IUnit abilityUser)
    {
        _abilityUser = abilityUser;
        AbilityId = Guid.NewGuid().ToString();
    }

}