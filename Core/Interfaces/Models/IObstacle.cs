namespace BoH.Interfaces;

/// <summary>
/// Интерфейс, представляющий препятствие на игровом поле.
/// </summary>
public interface IObstacle
{
    /// <summary>
    /// Уникальный идентификатор препятствия.
    /// </summary>
    string ObstacleId { get; }
}
