using System;
using System.Collections.ObjectModel;
using BoH.Interfaces;
using BoH.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoH.GUI.ViewModels;


public partial class GameBoardViewModel : ViewModelBase
{
    [ObservableProperty]
    private GameBoard _gameBoard;

    [ObservableProperty]
    private ObservableCollection<CellViewModel> _cells = [];

    [ObservableProperty] private int _width;
    [ObservableProperty] private int _height;
    
    private readonly ITurnManager _turnManager;
    
    public GameBoardViewModel(IGameBoard gameBoard, ITurnManager turnManager)
    {
        GameBoard = gameBoard as GameBoard ?? throw new ArgumentNullException(nameof(gameBoard));
        Width = GameBoard.Width;
        Height = GameBoard.Height;
        foreach(var cell in gameBoard.Cells)
        {
            Cells.Add(new CellViewModel(cell));
        }

        foreach (var cellVm in Cells)
        {
            cellVm.OnCellClicked += HandleCellSelection;
        }
        
        _turnManager = turnManager ?? throw new ArgumentNullException(nameof(turnManager));
    }

    public void HandleCellSelection()
    {
        
        Debug_GbvmSeesClick();
    }

    public void Debug_GbvmSeesClick()
    {
        Console.WriteLine("GBVM sees the click");
    }
}