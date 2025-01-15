namespace BoH.GUI.ViewModels
{
    using BoH.Interfaces;
    using BoH.Units;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Основная ViewModel для главного окна.
    /// </summary>
    public class MainWindowViewModel
    {
        private readonly IGameBoardService _gameBoardService;
        private readonly IGameBoard _gameBoard;

        /// <summary>
        /// ViewModel для игрового поля.
        /// </summary>
        public GameBoardViewModel GameBoardViewModel { get; }

        /// <summary>
        /// Конструктор для создания экземпляра <see cref="MainWindowViewModel"/>.
        /// </summary>
        /// <param name="gameBoardService">Сервис для работы с игровым полем.</param>
        public MainWindowViewModel(IGameBoardService gameBoardService)
        {
            _gameBoardService = gameBoardService ?? throw new ArgumentNullException(nameof(gameBoardService));

            // Создание команд
            var teams = CreateTeams();

            // Генерация игрового поля
            try
            {
                _gameBoard = _gameBoardService.GenerateGameBoard(8, 8, teams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка генерации игрового поля: {ex.Message}");
                throw;
            }

            // Создание GameBoardViewModel
            GameBoardViewModel = new GameBoardViewModel(_gameBoard);
        }

        /// <summary>
        /// Логика для сохранения текущего состояния игры.
        /// </summary>
        private void SaveGame()
        {
            // Реализовать логику сохранения игры
            Console.WriteLine("Игра сохранена");
        }

        /// <summary>
        /// Логика для загрузки ранее сохраненной игры.
        /// </summary>
        private void LoadGame()
        {
            // Реализовать логику загрузки игры
            Console.WriteLine("Игра загружена");
        }

        private Dictionary<string, List<IUnit>> CreateTeams()
        {
            return new Dictionary<string, List<IUnit>>
            {
                ["Rus"] = new List<IUnit>
                {
                    new RusArcher(), new RusArcher(),
                    new RusWarrior(), new RusWarrior(),
                    new RusWarrior(), new RusWarrior()
                },
                ["Lizard"] = new List<IUnit>
                {
                    new LizardArcher(), new LizardArcher(),
                    new LizardWarrior(), new LizardWarrior(),
                    new LizardWarrior(), new LizardWarrior()
                }
            };
        }
    }
}
