namespace BoH.GUI.ViewModels;

using System;
using BoH.Interfaces;
using BoH.Models;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class CellViewModel : ObservableObject, IIconHolder
{
    [ObservableProperty]
    private Cell _cell; // When needed to get data from _cell write Cell instead, i.e. Cell.Icon

    public string Icon { get; set; } = string.Empty;
    public (int X, int Y) Position {get; set;}
    public IIconHolder? Content {get; set;}

    public CellViewModel(Cell cell)
    {
        _cell = cell ?? throw new ArgumentNullException("No cell model provided to CellViewModel");
        UpdateViewModel();
    }

    public void UpdateViewModel()
    {
        Icon = Cell.Icon;
        Position = Cell.Position;
        Content = Cell.Content;
    }

    public void OnClicked()
    {
        // TODO 
    }
}