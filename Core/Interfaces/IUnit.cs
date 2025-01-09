namespace BoH.Interfaces;

/// <summary>
/// Представляет юнита в игре с базовыми характеристиками и действиями.
/// </summary>
public interface IUnit
{
    /// <summary>
    /// Кортеж-текущая позиция юнита на игровом поле.
    /// </summary>
    /// <example>
    /// (int X, int Y) position = (5, 10); // пример использования кортежа
    /// </example>
    public (int X, int Y) Position { get; set; }

    /// <summary>
    /// Уникальный идентификатор юнита.
    /// </summary>
    public string UnitId { get; }

    /// <summary>
    /// Идентификатор игрока, которому принадлежит юнит.
    /// </summary>
    public string OwnerId { get; }

    /// <summary>
    /// Иконка, представляющая юнита на игровом поле.
    /// </summary>
    public char Icon { get; set; }

    /// <summary>
    /// Текущее количество очков здоровья юнита.
    /// </summary>
    public int Hp { get; set; }

    /// <summary>
    /// Скорость передвижения юнита (количество клеток за ход).
    /// </summary>
    public int Speed { get; set; }

    /// <summary>
    /// Урон, наносимый юнитом при атаке.
    /// </summary>
    public int Damage { get; set; }

    /// <summary>
    /// Значение защиты юнита, уменьшающее входящий урон.
    /// </summary>
    public int Defence { get; set; }

    /// <summary>
    /// Тип или класс юнита (например, пехота, танк и т.д.).
    /// </summary>
    public string UnitType { get; }

    /// <summary>
    /// Показывает, находится ли юнит в состоянии оглушения или не может выполнять действия.
    /// </summary>
    public bool IsStunned { get; set; }

    /// <summary>
    /// Коллекция способностей, которыми обладает юнит.
    /// </summary>
    public List<IAbility> Abilities { get; }

    /// <summary>
    /// Применяет урон к юниту, уменьшая его очки здоровья.
    /// </summary>
    /// <param name="amount">Количество урона для применения.</param>
    void TakeDamage(int amount);

    /// <summary>
    /// Перемещает юнита в новую позицию на игровом поле.
    /// </summary>
    /// <param name="newPosition">Новая позиция, в которую нужно переместить юнита.</param>
    void Move((int X, int Y) newPosition);

    /// <summary>
    /// Вычисляет количество урона, который юнит наносит при атаке.
    /// </summary>
    /// <returns>Вычисленный урон.</returns>
    int CalculateAttackDamage();
}
