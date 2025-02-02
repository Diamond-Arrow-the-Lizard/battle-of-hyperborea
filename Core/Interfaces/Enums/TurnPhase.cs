namespace BoH.Interfaces;

/// <summary>
/// Текущая фаза хода юнита.
/// </summary>
public enum TurnPhase
{
    Movement, // Передвижение
    Action, // Атака/применение способности
    End // Юнит закончил свой ход 
}