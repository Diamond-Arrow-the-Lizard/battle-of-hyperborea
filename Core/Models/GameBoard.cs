namespace BoH.Models;

using BoH.Interfaces;

/// <summary>
/// Реализует игровое поле с размерами 10x10 и позволяет управлять юнитами, препятствиями и клетками.
/// </summary>
/// <example>
/// Пример создания объекта игрового поля и установки юнитов:
/// <code>
/// var gameBoard = new GameBoard(); // Создание нового объекта игрового поля
/// 
/// // Устанавливаем юнит в клетку (2, 3)
/// gameBoard[2, 3] = someUnit;
/// 
/// // Устанавливаем препятствие в клетку (5, 5)
/// gameBoard.SetObstacle(5, 5);
/// 
/// // Проверяем тип клетки (5, 5)
/// var cellType = gameBoard.GetCellType(5, 5); // Ожидаем CellType.Obstacle
/// 
/// // Проверяем доступность клетки (5, 5)
/// bool isAvailable = gameBoard.IsCellAvailable(5, 5); // Ожидаем false, так как там препятствие
/// </code>
/// </example>
public class GameBoard : IGameBoard
{
    private readonly IUnit?[,] _units;      // Массив для хранения юнитов на поле
    private readonly CellType[,] _cellTypes; // Массив для типов клеток (пусто, юнит, препятствие)

    /// <summary>
    /// Размер игрового поля по ширине (количество столбцов).
    /// </summary>
    public int Width { get; } = 10;

    /// <summary>
    /// Размер игрового поля по высоте (количество строк).
    /// </summary>
    public int Height { get; } = 10;

    /// <summary>
    /// Конструктор класса GameBoard, инициализирует поле размером 10x10, заполняя его пустыми клетками.
    /// </summary>
    public GameBoard()
    {
        _units = new IUnit?[Width, Height];
        _cellTypes = new CellType[Width, Height];

        // Инициализация поля пустыми клетками
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _cellTypes[x, y] = CellType.Empty;
            }
        }
    }

    /// <summary>
    /// Возвращает или задаёт объект (юнита), расположенный в указанной клетке игрового поля.
    /// </summary>
    /// <param name="x">Координата X (столбец).</param>
    /// <param name="y">Координата Y (строка).</param>
    /// <returns>Юнит в клетке или null, если клетка пуста.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если координаты выходят за пределы поля.</exception>
    /// <exception cref="InvalidOperationException">Если пытаются поместить юнит на клетку с препятствием.</exception>
    public IUnit? this[int x, int y]
    {
        get
        {
            ValidateCoordinates(x, y);
            return _units[x, y];
        }
        set
        {
            ValidateCoordinates(x, y);

            // Нельзя разместить юнит в клетке с препятствием
            if (_cellTypes[x, y] == CellType.Obstacle)
                throw new InvalidOperationException("Нельзя разместить юнит на клетке с препятствием.");

            _units[x, y] = value;
            _cellTypes[x, y] = value != null ? CellType.Unit : CellType.Empty;
        }
    }

    /// <summary>
    /// Возвращает тип содержимого клетки по координатам.
    /// </summary>
    /// <param name="x">Координата X (столбец).</param>
    /// <param name="y">Координата Y (строка).</param>
    /// <returns>Тип содержимого клетки (пусто, юнит, препятствие).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если координаты выходят за пределы поля.</exception>
    public CellType GetCellType(int x, int y)
    {
        ValidateCoordinates(x, y);
        return _cellTypes[x, y];
    }

    /// <summary>
    /// Проверяет, является ли клетка доступной для перемещения.
    /// Клетка считается доступной, если она пуста или на ней стоит юнит.
    /// </summary>
    /// <param name="x">Координата X (столбец).</param>
    /// <param name="y">Координата Y (строка).</param>
    /// <returns>True, если клетка доступна для перемещения (пустая или с юнитом); иначе false.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Если координаты выходят за пределы поля.</exception>
    public bool IsCellAvailable(int x, int y)
    {
        ValidateCoordinates(x, y);
        return _cellTypes[x, y] == CellType.Empty || _cellTypes[x, y] == CellType.Unit;
    }

    /// <summary>
    /// Проверяет, находятся ли координаты в пределах игрового поля.
    /// </summary>
    /// <param name="x">Координата X (столбец).</param>
    /// <param name="y">Координата Y (строка).</param>
    /// <exception cref="ArgumentOutOfRangeException">Если координаты выходят за пределы поля.</exception>
    private void ValidateCoordinates(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException($"Координаты ({x}, {y}) выходят за пределы игрового поля.");
    }

    /// <summary>
    /// Устанавливает препятствие в указанной клетке.
    /// Препятствие не может содержать юнита.
    /// </summary>
    /// <param name="x">Координата X (столбец).</param>
    /// <param name="y">Координата Y (строка).</param>
    /// <exception cref="ArgumentOutOfRangeException">Если координаты выходят за пределы поля.</exception>
    public void SetObstacle(int x, int y)
    {
        ValidateCoordinates(x, y);
        _cellTypes[x, y] = CellType.Obstacle;
        _units[x, y] = null; // Препятствие не может содержать юнита
    }
}