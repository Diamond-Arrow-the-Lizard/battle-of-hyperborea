namespace BoH.Interfaces;

/// <summary>
/// Интерфейс для управления игровым процессом.
/// Определяет основные методы, необходимые для запуска, управления и завершения игры.
/// </summary>
public interface IGameController
{

    /// <summary>
    /// Запускает игровой процесс.
    /// Инициализирует игру, включая подготовку игроков и игрового поля.
    /// </summary>
    /// <param name="width">Ширина игрового поля (количество столбцов).</param>
    /// <param name="length">Длина игрового поля (количество строк).</param>
    /// <param name="players">Команды игроков.</param>
    /// <returns>Новое поле для начала игры</returns>
    IGameBoard StartGame(int width, int length, IPlayer[] players);

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
