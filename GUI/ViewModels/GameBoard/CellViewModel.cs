using Avalonia.Media;

namespace BoH.GUI.ViewModels;

using System;
using BoH.Interfaces;
using BoH.Models;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class CellViewModel : ViewModelBase
{
    [ObservableProperty] private Cell _cell;
    [ObservableProperty] private IBrush _background = new SolidColorBrush(Colors.Gray);
    public string Icon => Cell.Icon;
    public event Action<CellViewModel>? OnCellClicked;
    

    public CellViewModel(int x, int y)
    {
        Cell = new Cell((x, y));
    }

    public CellViewModel(ICell cell)
    {
        Cell = cell as Cell ?? throw new ArgumentNullException(nameof(cell));
    }


    private void DebugCommandClick()
    {
        Console.WriteLine("Button was clicked");
    }


    public void HandleClick()
    {
        OnCellClicked?.Invoke(this);
        UpdateViewModel();
        DebugCommandClick();
    }

    public void UpdateViewModel()
    {
        if (Cell.Content is IUnit unit)
        {
            if (unit.IsDead == true)
            {
                Cell.UpdateIcon("X");
            }
        }
        else
        {
            Cell.UpdateIcon();
        }
        OnPropertyChanged(nameof(Icon));
    }
}