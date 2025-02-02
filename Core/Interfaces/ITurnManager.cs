namespace BoH.Interfaces;

/// <summary>
/// Управляет очередью ходов и переключением между игроками в рамках игрового раунда.
/// </summary>
public interface ITurnManager
{
    /// <summary>
    /// Событие, возникающее при успешном завершении хода игрока.
    /// </summary>
    /// <remarks>
    /// Срабатывает после выполнения следующих действий:
    /// <list type="number">
    /// <item><description>Проверки завершения всех фаз текущим игроком</description></item>
    /// <item><description>Применения пассивных способностей юнитов</description></item>
    /// <item><description>Переключения активного игрока</description></item>
    /// <item><description>Сброса фаз юнитов нового игрока</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Пример подписки на событие:
    /// <code>
    /// turnManager.OnTurnEnd += nextPlayer => 
    ///     Console.WriteLine($"Ход переходит к {nextPlayer.Team}");
    /// </code>
    /// </example>
    event Action<IPlayer> OnTurnEnd;

    /// <summary>
    /// Событие, возникающее при начале нового хода игрока.
    /// </summary>
    /// <remarks>
    /// Срабатывает непосредственно перед началом хода:
    /// <list type="number">
    /// <item><description>После завершения предыдущего хода</description></item>
    /// <item><description>После сброса состояний юнитов</description></item>
    /// <item><description>До начала действий нового игрока</description></item>
    /// </list>
    /// </remarks>
    event Action<IPlayer> OnTurnStart;

    /// <summary>
    /// Инициализирует новый игровой раунд.
    /// </summary>
    /// <param name="firstPlayer">Игрок, который будет ходить первым в раунде.</param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="firstPlayer"/> равен null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="firstPlayer"/> не является участником игры.
    /// </exception>
    /// <remarks>
    /// Выполняет:
    /// <list type="number">
    /// <item><description>Сброс состояний менеджера ходов</description></item>
    /// <item><description>Инициализацию очереди ходов</description></item>
    /// <item><description>Активацию первого игрока</description></item>
    /// </list>
    /// </remarks>
    void StartNewRound(IPlayer firstPlayer);

    /// <summary>
    /// Завершает текущий ход и переключает управление на следующего игрока.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается в случаях:
    /// <list type="bullet">
    /// <item><description>Попытка завершить ход до начала раунда</description></item>
    /// <item><description>Не все юниты завершили свои фазы</description></item>
    /// </list>
    /// </exception>
    /// <remarks>
    /// Последовательность выполнения:
    /// <list type="number">
    /// <item><description>Проверка условий завершения хода</description></item>
    /// <item><description>Применение пассивных способностей</description></item>
    /// <item><description>Переключение активного игрока</description></item>
    /// <item><description>Сброс фаз юнитов нового игрока</description></item>
    /// <item><description>Вызов события OnTurnEnd</description></item>
    /// <item><description>Вызов события OnTurnStart для нового игрока</description></item>
    /// </list>
    /// </remarks>
    void EndTurn();
}