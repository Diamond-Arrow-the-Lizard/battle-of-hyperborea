namespace BoH.GUI.ViewModels
{
    using BoH.Interfaces;
    using BoH.Models;
    using System.Collections.ObjectModel;

    /// <summary>
    /// ViewModel для игрового поля.
    /// </summary>
    public class GameBoardViewModel
    {
        private readonly IGameBoard _gameBoard;

        /// <summary>
        /// Коллекция ViewModel для клеток игрового поля.
        /// </summary>
        public ObservableCollection<ObservableCollection<CellViewModel>> Cells { get; }

        /// <summary>
        /// Конструктор для создания ViewModel игрового поля.
        /// </summary>
        /// <param name="gameBoard">Сервис для работы с игровым полем.</param>
        public GameBoardViewModel(IGameBoard gameBoard)
        {
            _gameBoard = gameBoard;

            // Инициализация коллекции клеток ViewModel
            Cells = new ObservableCollection<ObservableCollection<CellViewModel>>();

            // Преобразуем все клетки в двумерный массив и создаем для каждой клетки CellViewModel
            for (int i = 0; i < _gameBoard.Height; i++)
            {
                var row = new ObservableCollection<CellViewModel>();

                for (int j = 0; j < _gameBoard.Width; j++)
                {
                    var cell = _gameBoard[i, j]; // Получаем клетку из матрицы
                    if (cell is Cell c)
                        row.Add(new CellViewModel(c)); // Создаем ViewModel для клетки и добавляем в строку
                }

                Cells.Add(row); // Добавляем строку в коллекцию
            }
        }

        /// <summary>
        /// Получить все клетки игрового поля.
        /// </summary>
        /// <returns>Коллекция ViewModel всех клеток.</returns>
        public ObservableCollection<ObservableCollection<CellViewModel>> GetCells()
        {
            return Cells;
        }

        /// <summary>
        /// Получить клетку по координатам.
        /// </summary>
        /// <param name="x">Координата X клетки.</param>
        /// <param name="y">Координата Y клетки.</param>
        /// <returns>ViewModel клетки по заданным координатам.</returns>
        public CellViewModel? GetCell(int x, int y)
        {
            if (x >= 0 && x < _gameBoard.Width && y >= 0 && y < _gameBoard.Height)
            {
                return Cells[y][x]; // Возвращаем клетку из коллекции, используя координаты
            }
            return null;
        }
    }
}
