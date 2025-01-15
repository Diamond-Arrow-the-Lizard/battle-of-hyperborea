namespace BoH.GUI.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BoH.Services;

    /// <summary>
    /// ViewModel для кнопки сохранения игрового поля.
    /// </summary>
    public class SaveButtonViewModel
    {
        private readonly GameBoardService _gameBoardService;

        /// <summary>
        /// Команда для сохранения игрового поля.
        /// </summary>
        public ICommand SaveGameCommand { get; }

        /// <summary>
        /// Конструктор для создания экземпляра <see cref="SaveButtonViewModel"/>.
        /// </summary>
        /// <param name="gameBoardService">Сервис для работы с игровым полем.</param>
        public SaveButtonViewModel(GameBoardService gameBoardService)
        {
            _gameBoardService = gameBoardService;
            SaveGameCommand = new RelayCommand(async () => await SaveGameAsync());
        }

        /// <summary>
        /// Перезаписывает текущее игровое поле.
        /// </summary>
        /// <returns>Задача, которая выполняет сохранение.</returns>
        private async Task SaveGameAsync()
        {
            await _gameBoardService.DeleteGameBoardAsync();
            await _gameBoardService.SaveGameBoardAsync();
        }
    }
}
