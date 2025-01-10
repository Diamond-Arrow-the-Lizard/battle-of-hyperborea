namespace BoH.Models;

using System;
using BoH.Interfaces;

/// <summary>
/// Реализация препятствия на игровом поле.
/// </summary>
public class Obstacle : IObstacle
{
    /// <summary>
    /// Уникальный идентификатор препятствия. Генерируется автоматически.
    /// </summary>
    public string ObstacleId { get; }

    /// <summary>
    /// Графическое представление препятствия (символ). 
    /// По умолчанию используется символ 'B'.
    /// </summary>
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

    /// <summary>
    /// Инициализирует новый экземпляр препятствия с автоматически сгенерированным идентификатором.
    /// </summary>
    /// <param name="icon">Иконка препятствия (необязательный параметр, по умолчанию 'B').</param>
    public Obstacle(char icon = 'B')
    {
        ObstacleId = Guid.NewGuid().ToString();
        Icon = icon;
    }
}
