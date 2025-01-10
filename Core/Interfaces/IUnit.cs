namespace BoH.Interfaces;

/// <summary>
/// Интерфейс, представляющий боевого юнита.
/// </summary>
public interface IUnit
{
    /// <summary>
    /// Уникальный идентификатор юнита.
    /// </summary>
    string UnitId { get; }

    /// <summary>
    /// Название команды, которой принадлежит юнит.
    /// </summary>
    string Team { get; }

    /// <summary>
    /// Иконка, представляющая юнита на игровом поле.
    /// </summary>
    char Icon { get; set; }

    /// <summary>
    /// Текущее количество очков здоровья юнита.
    /// </summary>
    int Hp { get; set; }

    /// <summary>
    /// Скорость передвижения юнита (количество клеток за ход).
    /// </summary>
    int Speed { get; set; }

    /// <summary>
    /// Количество бросаемых кубов, бросаемых при атаке (D6).
    /// </summary>
    int DamageDices { get; set; }

    /// <summary>
    /// Значение защиты юнита, уменьшающее входящий урон.
    /// </summary>
    int Defence { get; set; }

    /// <summary>
    /// Тип или класс юнита (например, пехота, танк и т.д.).
    /// </summary>
    UnitType UnitType { get; }

    /// <summary>
    /// Показывает, находится ли юнит в состоянии оглушения или не может выполнять действия.
    /// </summary>
    bool IsStunned { get; set; }

    /// <summary>
    /// Показывает, жив ли юнит.
    /// </summary>
    bool IsDead { get; }

    /// <summary>
    /// Коллекция способностей, которыми обладает юнит.
    /// </summary>
    List<IAbility> Abilities { get; }

    /// <summary>
    /// Текущая позиция юнита на игровом поле.
    /// </summary>
    (int X, int Y) Position { get; set; }

    /// <summary>
    /// Применяет урон к юниту, уменьшая его очки здоровья.
    /// </summary>
    /// <param name="amount">Количество урона для применения.</param>
    void TakeDamage(int amount);

    /// <summary>
    /// Применяет лечение, увеличивая очки здоровья юнита.
    /// </summary>
    /// <param name="amount">Количество восстанавливаемого здоровья.</param>
    void Heal(int amount);

    /// <summary>
    /// Перемещает юнита на игровом поле.
    /// </summary>
    /// <param name="newPosition">Новая позиция юнита.</param>
    void Move((int X, int Y) newPosition);

    /// <summary>
    /// Проверяет, может ли юнит переместиться.
    /// </summary>
    /// <returns>true, если перемещение возможно; иначе false.</returns>
    bool CanMove();

    /// <summary>
    /// Вычисляет урон от атаки юнита.
    /// </summary>
    /// <returns>Значение урона.</returns>
    int CalculateAttackDamage();

    /// <summary>
    /// Выполняет атаку по другому юниту.
    /// </summary>
    /// <param name="target">Целевой юнит для атаки.</param>
    void Attack(IUnit target);

    /// <summary>
    /// Событие, вызываемое при смерти юнита.
    /// </summary>
    event Action<IUnit> OnDeath;
}
