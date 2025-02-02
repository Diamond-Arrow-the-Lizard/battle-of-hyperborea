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
    /// <param name="teams">Словарь, в котором ключ — название команды, а значение — список юнитов этой команды.</param>
    /// <param name="players">Список игроков, которым будут распределены юниты.</param>
    /// <returns>Экземпляр <see cref="IGameBoard"/>, представляющий игровое поле.</returns>
    /// <exception cref="ArgumentException">Если передано меньше двух команд или их больше двух.</exception>
    /// <exception cref="InvalidOperationException">Если число юнитов превышает допустимый лимит для игрового поля.</exception>
    IGameBoard GenerateGameBoard(int width, int height, IEnumerable<IUnit> units, ref IPlayer[] players);

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
