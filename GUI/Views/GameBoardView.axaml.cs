namespace BoH.GUI.Views
{
    using Avalonia.Controls;
    using BoH.GUI.ViewModels;
    using System;

    public partial class GameBoardView : UserControl
    {
        public GameBoardView()
        {
            InitializeComponent();
            if (Avalonia.Application.Current != null)
                this.DataContext = ((App)Avalonia.Application.Current).GetService<GameBoardViewModel>();
            else throw new InvalidOperationException("ServiceProvider is not initialized.");
        }
    }
}
