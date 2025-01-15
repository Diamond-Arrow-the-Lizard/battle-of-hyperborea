namespace BoH.GUI.Views
{
    using Avalonia.Controls;
    using BoH.GUI.ViewModels;
    using BoH.Models;

    public partial class GameBoardView : UserControl
    {
        public GameBoardView()
        {
            InitializeComponent();

            // Создание игрового поля
            var gameBoard = new GameBoard(10, 10); // 10x10 поле
            DataContext = new GameBoardViewModel(gameBoard);
        }
    }
}
