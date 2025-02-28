using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Media;
using Avalonia.Threading;
using BoH.Interfaces;
using BoH.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoH.GUI.ViewModels;


public partial class GameBoardViewModel : ViewModelBase
{
    [ObservableProperty] private GameBoard _gameBoard;
    [ObservableProperty] private ObservableCollection<CellViewModel> _cells = [];
    [ObservableProperty] private int _width;
    [ObservableProperty] private int _height;
    [ObservableProperty] private List<ICell> _scannedCells = [];
    
    private readonly ITurnManager _turnManager;
    private readonly IGameBoardService _gameBoardService;
    
    
    public GameBoardViewModel(IGameBoard gameBoard, 
        ITurnManager turnManager, 
        IGameBoardService gameBoardService)
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
        _gameBoardService = gameBoardService ?? throw new ArgumentNullException(nameof(gameBoardService));
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
                ScannedCells =
                    _turnManager.ProcessScanner(unit.CurrentTurnPhase != TurnPhase.Action ? null : unit.Abilities[0]);
                
                HighlightCells(ScannedCells);
            }
            else
            {
                IUnit unit = _turnManager.SelectedUnit;
                _turnManager.ProcessPlayerAction(ScannedCells, cellViewModel.Cell);
                HighlightCells(null);
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
        IUnit? unit = cellViewModel.Cell.Content as IUnit;
        if (unit != null)
            Console.WriteLine(unit.CurrentTurnPhase);
    }

    private void UpdateGameBoard()
    {
        foreach (var cell in Cells)
        {
            cell.UpdateViewModel(); 
        }
    }
    
    private void HighlightCells(List<ICell>? scannedCells)
    {
        foreach (var cellVm in Cells)
        {
            if (scannedCells != null)
            {
                cellVm.Background = scannedCells.Contains(cellVm.Cell) ? 
                    new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                cellVm.Background = new SolidColorBrush(Colors.Gray);
            }
        }
    }

}