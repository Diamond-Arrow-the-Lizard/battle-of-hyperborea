namespace BoH.Interfaces;

/// <summary>
/// Интерфейс, дающий классу иконку, показывающкю его на поле.
/// </summary>
public interface IIconHolder
{
    /// <summary>
    /// Иконка, показывающая объект на поле.
    /// </summary>
    string Icon { get; }
}