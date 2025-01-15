namespace BoH.GUI.Views
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using BoH.GUI.ViewModels;

    /// <summary>
    /// Представляет View для клетки игрового поля.
    /// </summary>
    public partial class CellView : UserControl
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="CellView"/>.
        /// </summary>
        public CellView()
        {
            InitializeComponent();
            DataContext = new CellViewModel(null!); // Для корректной компиляции
        }
    }
}
