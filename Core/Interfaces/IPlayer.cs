namespace BoH.Interfaces;

/// <summary>
/// Представляет игрока в игре, управляющего набором юнитов.
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Уникальный идентификатор игрока.
    /// </summary>
    /// <remarks>
    /// Генерируется автоматически при создании игрока.
    /// Гарантирует уникальность в рамках одной игровой сессии.
    /// </remarks>
    string Id { get; }

    /// <summary>
    /// Название команды, к которой принадлежит игрок.
    /// </summary>
    /// <remarks>
    /// Используется для визуального отличия команд и логики взаимодействия.
    /// Должно быть уникальным в рамках одной игры.
    /// </remarks>
    string Team { get; }

    /// <summary>
    /// Список юнитов, принадлежащих игроку.
    /// </summary>
    /// <remarks>
    /// Предоставляет доступ только для чтения к коллекции юнитов.
    /// Для модификации коллекции используйте методы AddUnit и RemoveUnit.
    /// </remarks>
    IReadOnlyList<IUnit> Units { get; }

    /// <summary>
    /// Проверяет, есть ли у игрока живые юниты.
    /// </summary>
    /// <remarks>
    /// Используется для определения условий победы/поражения.
    /// Возвращает true, если хотя бы один юнит жив.
    /// </remarks>
    bool HasAliveUnits { get; }

    /// <summary>
    /// Добавляет нового юнита в команду игрока.
    /// </summary>
    /// <param name="unit">Добавляемый юнит.</param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если передаваемый юнит равен null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если юнит с таким ID уже существует в команде.
    /// </exception>
    void AddUnit(IUnit unit);

    /// <summary>
    /// Удаляет юнит из команды игрока по его идентификатору.
    /// </summary>
    /// <param name="unitId">Идентификатор удаляемого юнита.</param>
    /// <returns>
    /// true, если юнит был успешно удалён; 
    /// false, если юнит с указанным ID не найден.
    /// </returns>
    bool RemoveUnit(string unitId);

    /// <summary>
    /// Подготавливает юнитов игрока к новому ходу.
    /// </summary>
    /// <remarks>
    /// Выполняет сброс состояния всех живых юнитов:
    /// - Устанавливает фазу хода в <see cref="TurnPhase.Movement"/>
    /// - Снимает временные эффекты (например, оглушение)
    /// - Сбрасывает флаги состояния
    /// </remarks>
    void ResetUnitsForNewTurn();
}