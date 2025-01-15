namespace BoH.GUI.ViewModels
{
    using BoH.Interfaces;
    using BoH.Services;
    using BoH.Models;
    using System.ComponentModel;
    using System;
    using System.Collections.Generic;
    using BoH.Units;

    /// <summary>
    /// Основная ViewModel для главного окна, которая включает все необходимые ViewModel-и для кнопок и игрового поля.
    /// </summary>
    public class MainWindowViewModel 
    {
        
        private readonly IGameBoardService _gameBoardService;

        private GameBoard gameBoard = new GameBoard(1, 1);
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

            Dictionary<string, List<IUnit>> teams = new();
                teams["Rus"] = new List<IUnit>
            {
                new RusArcher(), new RusArcher(),
                new RusWarrior(), new RusWarrior(),
                new RusWarrior(), new RusWarrior()
            };
                teams["Lizard"] = new List<IUnit>
            {
                new LizardArcher(), new LizardArcher(),
                new LizardWarrior(), new LizardWarrior(),
                new LizardWarrior(), new LizardWarrior()
            };

            _gameBoardService = gameBoardService ?? throw new ArgumentNullException(nameof(gameBoardService));;
            GameBoardViewModel = new GameBoardViewModel(gameBoard);
        }
        
    }
    
}
