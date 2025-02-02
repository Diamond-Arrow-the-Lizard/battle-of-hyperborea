﻿namespace BoH.Models;

using System.ComponentModel;
using BoH.Interfaces;

/// <summary>
/// Базовая реализация боевого юнита.
/// </summary>
public class BaseUnit : IUnit
{
    /// <summary>
    /// Максимально возможное здоровье юнита.
    /// </summary>
    protected virtual int MaxHealth { get; } = 100;

    /// <inheritdoc/>
    public string UnitId { get; }

    /// <inheritdoc/>
    public string UnitName { get; } = "Юнит";

    /// <inheritdoc/>
    public string Team { get; } = "Dev";

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если символ некорректен (например, не является печатным).
    /// </exception>
    public char Icon
    {
        get => _icon;
        set
        {
            if (!char.IsLetterOrDigit(value) && !char.IsSymbol(value) && !char.IsPunctuation(value))
            {
                throw new ArgumentException("Иконка препятствия должна быть печатным символом.");
            }
            _icon = value;
        }
    }
    private char _icon;

    /// <inheritdoc/>
    public int Hp { get; set; } = 100;

    /// <inheritdoc/>
    public int Speed { get; set; } = 3;

    /// <inheritdoc/>
    public int DamageDices { get; set; } = 3;

    /// <inheritdoc/>
    public int Defence { get; set; } = 0;

    /// <inheritdoc/>
    public int Range { get; protected set; } = 1;

    /// <inheritdoc/>
    public UnitType UnitType { get; } = UnitType.Melee;

    /// <inheritdoc/>
    public bool IsStunned { get; set; } = false;

    /// <inheritdoc/>
    public bool IsDead { get; private set; } = false;

    /// <inheritdoc/>
    public TurnPhase CurrentTurnPhase { get; set; }

    /// <inheritdoc/>
    public List<IAbility> Abilities { get; } = new();

    /// <inheritdoc/>
    public (int X, int Y) Position { get; set; }

    /// <inheritdoc/>
    public event Action<IUnit>? OnDeath;

    /// <summary>
    /// Инициализирует новый экземпляр препятствия.
    /// </summary>
    /// <param name="icon">Иконка препятствия (необязательный параметр, по умолчанию 'B').</param>
    /// <param name="team">Фракция юнита (необязательный параметр, по умолчанию "Dev").</param>
    /// <param name="type">Тип атаки персонажа (необязательный параметр, по умолчанию Melee).</param>
    public BaseUnit(string unitName, char icon = 'T', string team = "Dev", UnitType type = UnitType.Melee)
    {
        UnitName = unitName;
        UnitId = Guid.NewGuid().ToString();
        UnitType = type;
        Team = team;
        Icon = icon;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Выбрасывается, если юнит мёртв.</exception>
    public void Heal(int amount)
    {
        if (IsDead) throw new InvalidOperationException("Мертвый юнит не может быть вылечен.");

        Hp = Math.Min(MaxHealth, Hp + amount);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"></exception>
    public void Move((int X, int Y) newPosition)
    {
        if (IsDead) throw new InvalidOperationException("Мертвый юнит не может двигаться.");
        if (IsStunned) throw new InvalidOperationException("Оглушенный юнит не может двигаться.");
        if (CurrentTurnPhase != TurnPhase.Movement) throw new InvalidOperationException("Юнит не в фазе передвижения.");

        Position = newPosition;
        ChangeTurnPhase();
    }

    /// <inheritdoc/>
    /// <returns>true, если перемещение возможно; иначе false.</returns>
    public bool CanMove() => !IsDead && !IsStunned && CurrentTurnPhase == TurnPhase.Movement;

    /// <inheritdoc/>
    /// <returns>Значение урона. Если юнит мёртв, возвращает 0.</returns>
    public int CalculateAttackDamage()
    {
        if (!IsDead)
        {
            int calculatedDamage = 0;
            Random rnd = new();
            for (int i = 0; i < DamageDices; i++)
            {
                calculatedDamage += rnd.Next(1, 7);
            }
            return calculatedDamage;
        }
        else return 0;
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public void Attack(IUnit target)
    {
        if (IsDead) throw new InvalidOperationException("Мертвый юнит не может атаковать.");
        if (CurrentTurnPhase != TurnPhase.Action) throw new InvalidOperationException("Юнит не в фазе действия.");
        if (target.IsDead) throw new InvalidOperationException("Нельзя атаковать мертвого юнита.");

        target.TakeDamage(CalculateAttackDamage());
        ChangeTurnPhase();
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidEnumArgumentException"/>
    public void ChangeTurnPhase()
    {
        CurrentTurnPhase = CurrentTurnPhase switch
        {
            TurnPhase.Movement => TurnPhase.Action,
            TurnPhase.Action => TurnPhase.End,
            TurnPhase.End => TurnPhase.Movement,
            _ => throw new InvalidEnumArgumentException("Неизвестная фаза хода юнита."),
        };
    }
}
