namespace BoH.Interfaces;

/// <summary>
/// Представляет способность, которая может быть использована юнитом.
/// </summary>
public interface IAbility
{
    /// <summary>
    /// Уникальный идентификатор способности.
    /// </summary>
    public string AbilityId { get; }

    /// <summary>
    /// Название способности.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Краткое описание способности.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Указывает, является ли способность активной (требует действия для использования) 
    /// или пассивной (всегда активна).
    /// </summary>
    public bool IsActive { get; }

    /// </summary>
    /// Кулдаун способности (для активных).
    /// </summary>
    int Coolown { set; get; }

    /// </summary>
    /// Дальность применения способности.
    /// </summary>
    int AbilityRange { get; set; }

    /// <summary>
    /// Выполняет способность.
    /// </summary>
    /// <param name="target">Целевой юнит или объект (опционально).</param>
    /// <returns>Возвращает `true`, если способность была успешно применена.</returns>
    bool Activate(IUnit? target = null);

    /// <summary>
    /// Обновляет состояние способности, если требуется, например, восстановление
    /// или влияние на окружающих.
    /// </summary>
    void Update();

    /// <summary>
    /// Событие при применении способности 
    /// </summary>
    event Action<IAbility> OnAbilityUsed;

    /// <summary>
    /// Событие при провале применения способности
    /// </summary>
    event Action<IAbility>? OnAbilityFailed;

    /// <summary>
    /// Событие при кулдауне способности 
    /// </summary>
    event Action<IAbility>? OnCooldown;

}
