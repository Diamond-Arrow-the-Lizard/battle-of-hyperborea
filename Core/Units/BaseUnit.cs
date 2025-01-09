﻿namespace BoH.Units;

using BoH.Interfaces;

/// <summary>
/// Базовая реализация боевого юнита.
/// </summary>
public class BaseUnit : IUnit
{
    /// <summary>
    /// Максимально возможное здоровье юнита.
    /// </summary>
    private readonly int _maxHealth = 100;

    /// <summary>
    /// Уникальный идентификатор юнита.
    /// </summary>
    public string UnitId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Название команды, которой принадлежит юнит.
    /// </summary>
    public string Team { get; } = "Dev";

    /// <summary>
    /// Иконка, представляющая юнита на игровом поле.
    /// </summary>
    public char Icon { get; set; } = 'T';

    /// <summary>
    /// Текущее количество очков здоровья юнита.
    /// </summary>
    public int Hp { get; set; } = 100;

    /// <summary>
    /// Скорость передвижения юнита (количество клеток за ход).
    /// </summary>
    public int Speed { get; set; } = 3;

    /// <summary>
    /// Урон, наносимый юнитом при атаке.
    /// </summary>
    public int Damage { get; set; } = 10;

    /// <summary>
    /// Значение защиты юнита, уменьшающее входящий урон.
    /// </summary>
    public int Defence { get; set; } = 0;

    /// <summary>
    /// Тип или класс юнита (например, пехота, танк и т.д.).
    /// </summary>
    public UnitType UnitType { get; } = UnitType.Melee;

    /// <summary>
    /// Показывает, находится ли юнит в состоянии оглушения или не может выполнять действия.
    /// </summary>
    public bool IsStunned { get; set; } = false;

    /// <summary>
    /// Показывает, жив ли юнит.
    /// </summary>
    public bool IsDead { get; private set; } = false;

    /// <summary>
    /// Коллекция способностей, которыми обладает юнит.
    /// </summary>
    public List<IAbility> Abilities { get; } = new();

    /// <summary>
    /// Текущая позиция юнита на игровом поле.
    /// </summary>
    public (int X, int Y) Position { get; set; }

    /// <summary>
    /// Событие, вызываемое при смерти юнита.
    /// </summary>
    public event Action<IUnit>? OnDeath;

    /// <summary>
    /// Применяет урон к юниту, уменьшая его очки здоровья.
    /// </summary>
    /// <param name="amount">Количество урона для применения.</param>
    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        int effectiveDamage = Math.Max(0, amount - Defence);
        Hp -= effectiveDamage;

        if (Hp <= 0)
        {
            Hp = 0;
            IsDead = true;
            OnDeath?.Invoke(this);
        }
    }

    /// <summary>
    /// Применяет лечение, увеличивая очки здоровья юнита.
    /// </summary>
    /// <param name="amount">Количество восстанавливаемого здоровья.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если юнит мёртв.</exception>
    public void Heal(int amount)
    {
        if (IsDead) throw new InvalidOperationException("Мертвый юнит не может быть вылечен.");

        Hp = Math.Min(_maxHealth, Hp + amount); 
    }

    /// <summary>
    /// Перемещает юнита на игровом поле.
    /// </summary>
    /// <param name="newPosition">Новая позиция юнита.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если юнит мёртв или оглушён.</exception>
    public void Move((int X, int Y) newPosition)
    {
        if (IsDead) throw new InvalidOperationException("Мертвый юнит не может двигаться.");
        if (IsStunned) throw new InvalidOperationException("Оглушенный юнит не может двигаться.");

        Position = newPosition;
    }

    /// <summary>
    /// Проверяет, может ли юнит переместиться.
    /// </summary>
    /// <returns>true, если перемещение возможно; иначе false.</returns>
    public bool CanMove() => !IsDead && !IsStunned;

    /// <summary>
    /// Вычисляет урон от атаки юнита.
    /// </summary>
    /// <returns>Значение урона. Если юнит мёртв, возвращает 0.</returns>
    public int CalculateAttackDamage() => IsDead ? 0 : Damage;

    /// <summary>
    /// Выполняет атаку по другому юниту.
    /// </summary>
    /// <param name="target">Целевой юнит для атаки.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если атакующий или цель мертвы.</exception>
    public void Attack(IUnit target)
    {
        if (IsDead) throw new InvalidOperationException("Мертвый юнит не может атаковать.");
        if (target.IsDead) throw new InvalidOperationException("Нельзя атаковать мертвого юнита.");

        target.TakeDamage(CalculateAttackDamage());
    }
}
