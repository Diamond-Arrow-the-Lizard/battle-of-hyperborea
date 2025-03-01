namespace BoH.Interfaces;

/// <summary>
/// Управляет очередью ходов, переключением между игроками и обработкой игровых действий.
/// </summary>
public interface ITurnManager
{
    public IPlayer CurrentPlayer { get; set; }
    public IUnit? SelectedUnit { get; set; }

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
    event Action<IUnit>? OnUnitSelected;

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
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если юнит недоступен для выбора.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если в клетке нет юнита.
    /// </exception>
    void SelectUnit(ICell unitCell);

    /// <summary>
    /// Обрабатывает сканирование клеток для указанного действия.
    /// </summary>
    /// <param name="usedAbility"> Используемая способность во время фазы действия (опционально). </param>
    /// <returns>Список клеток, доступных для выполнения действия.</returns>
    List<ICell> ProcessScanner(IAbility? usedAbility = null);

    /// <summary>
    /// Выполняет действие, выбранное игроком.
    /// </summary>
    /// <param name="availableCells">Список клеток, доступных для действия (опционально).</param>
    /// <param name="target">Цель действия (опционально).</param>
    /// <param name="usedAbility">Способность, используемая для действия (опционально).</param>
    /// <exception cref="InvalidDataException">
    /// Выбрасывается, если цель действия не является клеткой.
    /// </exception>
    void ProcessPlayerAction(
        List<ICell>? availableCells = null,
        object? target = null,
        IAbility? usedAbility = null);
}