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

    public GameBoardViewModel(IGameBoard gameBoard)
    {
        _gameBoard = gameBoard as GameBoard ?? throw new ArgumentNullException(nameof(gameBoard));
        foreach(var cell in gameBoard.Cells)
        {
            Cells.Add(new CellViewModel(cell));
        }
    }

}