namespace BoH.GUI.ViewModels;

using System;
using BoH.Interfaces;
using BoH.Models;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class CellViewModel : ViewModelBase
{
    [ObservableProperty] private Cell _cell;
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
        Cell.UpdateIcon();
        OnPropertyChanged(nameof(Icon));
    }
}