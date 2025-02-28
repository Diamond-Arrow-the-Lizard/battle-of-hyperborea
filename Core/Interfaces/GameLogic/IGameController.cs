namespace BoH.Interfaces;

/// <summary>
/// Интерфейс для управления игровым процессом.
/// Определяет основные методы, необходимые для запуска, управления и завершения игры.
/// </summary>
public interface IGameController
{
    /// <summary>
    /// Событие при выигрыше одной команды
    /// </summary>
    public event Action<IPlayer>? OnPlayerWinning;

    /// <summary>
    /// Проверяет, закончился ли ход игрока. 
    /// </summary>
    /// <param name="player">Текущий игрок.</param>
    /// <returns>True, елси походили все юниты игрока. </returns> 
    public bool CheckForTurnEnd(IPlayer player);

    /// <summary>
    /// Проверяет выполнение условий победы.
    /// Определяет, выполнены ли условия завершения игры, например, остались ли юниты только одной команды.
    /// </summary>
    /// <param name="players">Команды игроков.</param>
    /// <returns>True, елси выполнены условия победы. </returns> 
    bool CheckVictoryCondition(IPlayer[] players);

    /// <summary>
    /// Завершает игровой процесс.
    /// Выполняет действия по завершению игры.
    /// </summary>
    /// <returns>Асинхронная задача, которая завершается после завершения игры.</returns>
    Task EndGame();
}
