using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Threading;
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
    private List<ICell> _scannedCells = [];
    
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

    public void HandleCellSelection(CellViewModel cellViewModel)
    {
        Debug_GbvmSeesClick(cellViewModel);
        _turnManager.StartNewRound(_turnManager.CurrentPlayer);
        try
        {
            if (_turnManager.SelectedUnit == null)
            {
                _turnManager.SelectUnit(cellViewModel.Cell);
                IUnit unit = _turnManager.SelectedUnit ?? throw new ArgumentNullException(nameof(cellViewModel.Cell));
                _scannedCells =
                    _turnManager.ProcessScanner(unit.CurrentTurnPhase != TurnPhase.Action ? null : unit.Abilities[0]);
            }
            else
            {
                IUnit unit = _turnManager.SelectedUnit;
                _turnManager.ProcessPlayerAction(_scannedCells, cellViewModel.Cell);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        finally
        {
            UpdateGameBoard();
        }
    }

    private void Debug_GbvmSeesClick(CellViewModel cellViewModel)
    {
        Console.WriteLine("GBVM sees the click");
        Console.WriteLine($"{cellViewModel.Cell.Icon}");
    }

    private void UpdateGameBoard()
    {
        foreach (var cell in Cells)
        {
            cell.UpdateViewModel(); 
        }
    }
}