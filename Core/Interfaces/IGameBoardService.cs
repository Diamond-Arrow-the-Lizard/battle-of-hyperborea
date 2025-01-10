namespace BoH.Interfaces;

/// <summary>
/// Интерфейс для управления игровым полем.
/// </summary>
public interface IGameBoardService
{
    /// <summary>
    /// Генерирует новое игровое поле заданного размера.
    /// </summary>
    /// <param name="width">Ширина игрового поля (количество столбцов).</param>
    /// <param name="length">Длина игрового поля (количество строк).</param>
    /// <returns>Экземпляр <see cref="IGameBoard"/>, представляющий игровое поле.</returns>
    IGameBoard GenerateGameBoard(int width, int length);

    /// <summary>
    /// Добавляет объект в указанную клетку игрового поля.
    /// </summary>
    /// <param name="obj">Объект для добавления.</param>
    /// <param name="cell">Целевая клетка игрового поля.</param>
    void AddObjectToGameBoard(object? obj, ICell cell);

    /// <summary>
    /// Удаляет объект из указанной клетки игрового поля.
    /// </summary>
    /// <param name="obj">Объект для удаления.</param>
    /// <param name="cell">Целевая клетка игрового поля.</param>
    void RemoveObjectFromGameBoard(object? obj, ICell cell);

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
