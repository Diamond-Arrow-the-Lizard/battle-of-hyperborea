namespace BoH.GUI.ViewModels;

using BoH.Models;
using BoH.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Generic;

/// <summary>
/// ViewModel, представляющая игровое поле и его состояние.
/// </summary>
public class GameBoardViewModel
{
    private IGameBoard _gameBoard;

    /// <summary>
    /// Коллекция клеток игрового поля.
    /// </summary>
    public ObservableCollection<CellViewModel> Cells { get; } = new();

    /// <summary>
    /// Количество строк на игровом поле.
    /// </summary>
    public int RowCount => _gameBoard.Height;

    /// <summary>
    /// Количество столбцов на игровом поле.
    /// </summary>
    public int ColumnCount => _gameBoard.Width;

    /// <summary>
    /// Словарь с командами и юнитами в этих командах.
    /// </summary>
    private Dictionary<string, List<IUnit>> _teams = new();

    /// <summary>
    /// Создаёт новый экземпляр <see cref="GameFieldViewModel"/> с заданным игровым полем.
    /// </summary>
    /// <param name="gameBoard">Объект игрового поля.</param>
    public GameBoardViewModel(IGameBoard gameBoard)
    {
        _gameBoard = gameBoard; 

        // Инициализация ViewModel для каждой клетки
        for (int y = 0; y < _gameBoard.Height; y++)
        {
            for (int x = 0; x < _gameBoard.Width; x++)
            {
                Cells.Add(new CellViewModel(_gameBoard[x, y]));
            }
        }
    }
}