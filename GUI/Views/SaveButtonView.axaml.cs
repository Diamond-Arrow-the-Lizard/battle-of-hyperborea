namespace BoH.GUI.Views
{
    using Avalonia.Controls;
    using BoH.GUI.ViewModels;
    using System;

    /// <summary>
    /// Представляет View для кнопки сохранения игрового поля.
    /// </summary>
    public partial class SaveButtonView : UserControl
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="SaveButtonView"/>.
        /// </summary>
        public SaveButtonView()
        {
            InitializeComponent();
            if (Avalonia.Application.Current != null)
                this.DataContext = ((App)Avalonia.Application.Current).GetService<SaveButtonViewModel>();
            else throw new InvalidOperationException("ServiceProvider is not initialized.");
        }
    }
}
