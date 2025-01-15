namespace BoH.GUI.Views
{
    using Avalonia.Controls;
    using BoH.GUI.ViewModels;
    using BoH.Services;

    /// <summary>
    /// Главное окно приложения, которое включает в себя все элементы управления и привязки данных.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Инициализация сервиса для игрового поля
            var gameBoardService = new GameBoardService();

            // Привязка MainWindowViewModel к DataContext
            DataContext = new MainWindowViewModel(gameBoardService);
        }
    }
}
