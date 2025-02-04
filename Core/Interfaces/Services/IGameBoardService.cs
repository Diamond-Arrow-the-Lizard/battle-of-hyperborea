namespace BoH.Interfaces;

/// <summary>
/// Интерфейс для управления игровым полем.
/// </summary>
public interface IGameBoardService
{
    /// <summary>
    /// Генерирует новое игровое поле заданного размера, расставляет препятствия и юнитов.
    /// </summary>
    /// <param name="width">Ширина игрового поля (количество столбцов).</param>
    /// <param name="height">Высота игрового поля (количество строк).</param>
    /// <param name="units">Коллекция юнитов для размещения на поле.</param>
    /// <param name="players">Массив из двух игроков, которым будут распределены юниты.</param>
    /// <returns>Экземпляр <see cref="IGameBoard"/>, представляющий игровое поле.</returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается при:
    /// <list type="bullet">
    /// <item><description>Количестве игроков отличном от 2</description></item>
    /// <item><description>Количестве уникальных команд не равном 2</description></item>
    /// </list>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается при:
    /// <list type="bullet">
    /// <item><description>Недостаточном размере поля для размещения юнитов</description></item>
    /// <item><description>Обнаружении юнита с неизвестной командой</description></item>
    /// </list>
    /// </exception>
    IGameBoard GenerateGameBoard(int width, int height, IEnumerable<IUnit> units, IPlayer[] players);

    /// <summary>
    /// Добавляет объект в указанную клетку игрового поля.
    /// </summary>
    /// <param name="obj">Объект для добавления.</param>
    /// <param name="cell">Целевая клетка игрового поля.</param>
    void AddObjectToGameBoard(IIconHolder? obj, ICell cell);

    /// <summary>
    /// Удаляет объект из указанной клетки игрового поля.
    /// </summary>
    /// <param name="obj">Объект для удаления.</param>
    /// <param name="cell">Целевая клетка игрового поля.</param>
    void RemoveObjectFromGameBoard(IIconHolder? obj, ICell cell);

    /// <summary>
    /// Асинхронно сохраняет текущее состояние игрового поля.
    /// </summary>
    /// <returns>Задача, представляющая процесс сохранения.</returns>
    Task SaveGameBoardAsync();

    /// <summary>
    /// Асинхронно загружает состояние игрового поля.
    /// </summary>
    /// <returns>Задача, представляющая процесс загрузки.</returns>
    Task LoadGameBoardAsync();

    /// <summary>
    /// Асинхронно удаляет текущее состояние игрового поля.
    /// </summary>
    /// <returns>Задача, представляющая процесс удаления.</returns>
    Task DeleteGameBoardAsync();
}
