namespace BoH.GUI.ViewModels
{
    using BoH.Interfaces;
    using BoH.Services;
    using BoH.Models;
    using System.ComponentModel;

    /// <summary>
    /// Основная ViewModel для главного окна, которая включает все необходимые ViewModel-и для кнопок и игрового поля.
    /// </summary>
    public class MainWindowViewModel 
    {
        private GameBoard gameBoard = new GameBoard(5, 5);
        /// <summary>
        /// ViewModel для игрового поля.
        /// </summary>
        public GameBoardViewModel GameBoardViewModel { get; }

        /// <summary>
        /// ViewModel для кнопки сохранения.
        /// </summary>
        public SaveButtonViewModel SaveButtonViewModel { get; }

        /// <summary>
        /// ViewModel для кнопки загрузки.
        /// </summary>
        public LoadButtonViewModel LoadButtonViewModel { get; }

        /// <summary>
        /// Конструктор для создания экземпляра <see cref="MainWindowViewModel"/>.
        /// </summary>
        /// <param name="gameBoardService">Сервис для работы с игровым полем.</param>
        public MainWindowViewModel(GameBoardService gameBoardService)
        {
            GameBoardViewModel = new GameBoardViewModel(gameBoardService, gameBoard);
            SaveButtonViewModel = new SaveButtonViewModel(gameBoardService);
            LoadButtonViewModel = new LoadButtonViewModel(gameBoardService);
        }
    }
}
