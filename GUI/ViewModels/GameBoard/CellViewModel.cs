namespace BoH.GUI.ViewModels;

using System;
using BoH.Interfaces;
using BoH.Models;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class CellViewModel : ViewModelBase
{
    [ObservableProperty]
    private Cell _cell;

    public CellViewModel(int x, int y)
    {
        Cell = new Cell((x, y));
    }

    public CellViewModel(ICell cell)
    {
        Cell = cell as Cell ?? throw new ArgumentNullException(nameof(cell));
    }


    public void DebugCommandClick()
    {
        Console.WriteLine("Button was clicked");
    }


    public void HandleClick()
    {
        DebugCommandClick();
    }
}