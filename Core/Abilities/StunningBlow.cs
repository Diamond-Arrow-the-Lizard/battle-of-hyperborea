namespace BoH.Abilities;

using BoH.Interfaces;
using BoH.Models;

public class StunningBlow : IAbility
{
    /// <summary>
    /// Уникальный идентификатор способности.
    /// </summary>
    public string AbilityId { get; }

    /// <summary>
    /// Название способности.
    /// </summary>
    public string Name { get; } = "Ошеломляющий удар";

    /// <summary>
    /// Краткое описание способности.
    /// </summary>
    public string Description { get; } = "Оглушает противника при ударе. Бросается только один кубик";

    /// <summary>
    /// Указывает, является ли способность активной (требует действия для использования) 
    /// или пассивной (всегда активна).
    /// </summary>
    public bool IsActive { get; } = true;

    /// <summary>
    /// Кол-во ходов, которые нужно переждать перед повторным применением.
    /// </summary>
    public int Coolown { set; get; } = 0;

    /// <summary>
    /// Выполняет способность, если она активная.
    /// </summary>
    /// <param name="user">Юнит, использующий способность.</param>
    /// <param name="target">Целевой юнит или объект (опционально).</param>
    /// <returns>Возвращает `true`, если способность была успешно применена.</returns>
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

    /// <summary>
    /// Обновляет состояние способности, если требуется, например, восстановление
    /// или влияние на окружающих.
    /// </summary>
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