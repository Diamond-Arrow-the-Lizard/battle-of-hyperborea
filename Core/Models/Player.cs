namespace BoH.Models;

using BoH.Interfaces;
using System.Collections.ObjectModel;

/// <summary>
/// Представляет игрока и управляет его юнитами в ходе игры.
/// </summary>
public class Player : IPlayer
{
    private readonly List<IUnit> _units = new();

    /// <inheritdoc/>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <inheritdoc/>
    public string Team { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IUnit> Units => new ReadOnlyCollection<IUnit>(_units);

    /// <inheritdoc/>
    public bool HasAliveUnits => _units.Any(u => !u.IsDead);

    /// <summary>
    /// Инициализирует нового игрока.
    /// </summary>
    /// <param name="team">Название команды игрока.</param>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если название команды пустое или содержит только пробелы.
    /// </exception>
    public Player(string team)
    {
        if (string.IsNullOrWhiteSpace(team))
            throw new ArgumentException("Название команды не может быть пустым.", nameof(team));

        Team = team;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если юнит равен null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если юнит с таким ID уже существует в команде.
    /// </exception>
    public void AddUnit(IUnit unit)
    {
        if (unit == null)
            throw new ArgumentNullException(nameof(unit), "Юнит не может быть null.");

        if (_units.Any(u => u.UnitId == unit.UnitId))
            throw new InvalidOperationException($"Юнит с ID {unit.UnitId} уже существует в команде.");

        _units.Add(unit);
    }

    /// <inheritdoc/>
    public bool RemoveUnit(string unitId)
    {
        var unit = _units.FirstOrDefault(u => u.UnitId == unitId);
        return unit != null && _units.Remove(unit);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Сбрасывает фазу хода всех живых юнитов в состояние <see cref="TurnPhase.Movement"/>
    /// и снимает временные эффекты (например, оглушение).
    /// </remarks>
    public void ResetUnitsForNewTurn()
    {
        foreach (var unit in _units.Where(u => !u.IsDead))
        {
            unit.ResetTurnState();
            unit.IsStunned = false;
        }
    }
}