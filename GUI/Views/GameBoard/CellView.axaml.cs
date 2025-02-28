using Avalonia.Controls;
using BoH.GUI.ViewModels;
using BoH.Models;

namespace BoH.GUI.Views;

public partial class CellView : UserControl
{
    public CellView()
    {
        InitializeComponent();

        DataContext = new CellViewModel(new Cell((0, 0)));
    }
}