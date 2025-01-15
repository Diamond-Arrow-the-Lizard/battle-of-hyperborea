namespace BoH.GUI.Views
{
    using Avalonia.Controls;
    using BoH.GUI.ViewModels;
    using System;

    /// <summary>
    /// Представляет View для кнопки загрузки игрового поля.
    /// </summary>
    public partial class LoadButtonView : UserControl
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="LoadButtonView"/>.
        /// </summary>
        public LoadButtonView()
        {
            InitializeComponent();
            if (Avalonia.Application.Current != null)
                this.DataContext = ((App)Avalonia.Application.Current).GetService<LoadButtonViewModel>();
            else throw new InvalidOperationException("ServiceProvider is not initialized.");
        }
    }
}
