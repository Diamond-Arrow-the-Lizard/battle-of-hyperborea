namespace BoH.GUI.Views
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using BoH.GUI.ViewModels;
    using System;

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
            if (Avalonia.Application.Current != null)
                this.DataContext = ((App)Avalonia.Application.Current).GetService<CellViewModel>();
            else throw new InvalidOperationException("ServiceProvider is not initialized.");
        }
    }
}
