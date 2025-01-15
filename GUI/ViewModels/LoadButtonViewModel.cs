namespace BoH.GUI.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BoH.Services;

    /// <summary>
    /// ViewModel для кнопки загрузки игрового поля.
    /// </summary>
    public class LoadButtonViewModel
    {
        private readonly GameBoardService _gameBoardService;

        /// <summary>
        /// Команда для загрузки игрового поля.
        /// </summary>
        public ICommand LoadGameCommand { get; }

        /// <summary>
        /// Конструктор для создания экземпляра <see cref="LoadButtonViewModel"/>.
        /// </summary>
        /// <param name="gameBoardService">Сервис для работы с игровым полем.</param>
        public LoadButtonViewModel(GameBoardService gameBoardService)
        {
            _gameBoardService = gameBoardService;
            LoadGameCommand = new RelayCommand(async () => await LoadGameAsync());
        }

        /// <summary>
        /// Загружает сохранённое игровое поле.
        /// </summary>
        /// <returns>Задача, которая выполняет загрузку.</returns>
        private async Task LoadGameAsync()
        {
            await _gameBoardService.LoadGameBoardAsync();
        }
    }
}
