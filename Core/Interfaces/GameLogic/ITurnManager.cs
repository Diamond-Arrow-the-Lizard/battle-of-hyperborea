namespace BoH.Interfaces;

/// <summary>
/// Управляет очередью ходов, переключением между игроками и обработкой игровых действий.
/// </summary>
public interface ITurnManager
{
    /// <summary>
    /// Событие, возникающее при завершении хода текущего игрока.
    /// </summary>
    event Action<IPlayer>? OnTurnEnd;

    /// <summary>
    /// Событие, возникающее при начале хода нового игрока.
    /// </summary>
    event Action<IPlayer>? OnTurnStart;

    /// <summary>
    /// Событие, возникающее при выборе юнита для выполнения действий.
    /// </summary>
    event Action<IUnit, AvailableActions>? OnUnitSelected;

    /// <summary>
    /// Событие, возникающее при изменении состояния хода (например, после выполнения действия).
    /// </summary>
    event Action<IUnit>? OnTurnStateChanged;

    /// <summary>
    /// Начинает новый игровой раунд, инициализируя состояние хода для указанного игрока.
    /// </summary>
    /// <param name="firstPlayer">Игрок, который будет ходить первым в раунде.</param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="firstPlayer"/> равен null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="firstPlayer"/> не является участником игры.
    /// </exception>
    void StartNewRound(IPlayer firstPlayer);

    /// <summary>
    /// Завершает текущий ход, применяя пассивные способности и переключая активного игрока.
    /// </summary>
    void EndTurn();

    /// <summary>
    /// Выбирает юнита для выполнения действий на основе выбранной клетки.
    /// </summary>
    /// <param name="unitCell">Клетка, содержащая выбранного юнита.</param>
    /// <returns>Доступные действия для выбранного юнита.</returns>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если юнит недоступен для выбора.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если в клетке нет юнита.
    /// </exception>
    AvailableActions SelectUnit(ICell unitCell);

    /// <summary>
    /// Обрабатывает сканирование клеток для указанного действия.
    /// </summary>
    /// <param name="action">Тип действия (перемещение, атака, способность).</param>
    /// <returns>Список клеток, доступных для выполнения действия.</returns>
    List<ICell> ProcessScanner(ActionType action);

    /// <summary>
    /// Выполняет действие, выбранное игроком.
    /// </summary>
    /// <param name="action">Тип действия.</param>
    /// <param name="availableCells">Список клеток, доступных для действия (опционально).</param>
    /// <param name="target">Цель действия (опционально).</param>
    /// <param name="usedAbility">Способность, используемая для действия (опционально).</param>
    /// <exception cref="InvalidDataException">
    /// Выбрасывается, если цель действия не является клеткой.
    /// </exception>
    void ProcessPlayerAction(
        ActionType action, 
        List<ICell>? availableCells = null, 
        object? target = null, 
        IAbility? usedAbility = null);
}