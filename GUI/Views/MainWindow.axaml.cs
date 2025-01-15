namespace BoH.GUI.Views
{
    using System;
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
            if (Avalonia.Application.Current != null)
                this.DataContext = ((App)Avalonia.Application.Current).GetService<MainWindowViewModel>();
            else throw new InvalidOperationException("ServiceProvider is not initialized.");

        }
    }
}
