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
    [ObservableProperty] private IUnit? _selectedUnit = null;
    [ObservableProperty] private TurnPhase? _currentTurnPhase = null;
    [ObservableProperty] private bool? _isTurnEnded = null;
    
    
    // TODO changable ability
    public IAbility? SelectedAbility => SelectedUnit?.Abilities[0];
    
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
        if (IsTurnEnded == null)
        {
            _turnManager.StartNewRound(_turnManager.CurrentPlayer);
            IsTurnEnded = false;
        }
        
        try
        {
            if (_turnManager.SelectedUnit == null)
            {
                (int x, int y) cellPosition = FetchCoordinates(cellViewModel);
                _turnManager.SelectUnit(GameBoard[cellPosition.x, cellPosition.y]);
                SelectedUnit = _turnManager.SelectedUnit;
                CurrentTurnPhase = SelectedUnit?.CurrentTurnPhase;
                Console.WriteLine($"What TurnManager knows: {_turnManager.SelectedUnit} - {_turnManager.SelectedUnit?.CurrentTurnPhase}");
                ScannedCells =
                    _turnManager.ProcessScanner(SelectedUnit?.CurrentTurnPhase != TurnPhase.Action ? null : SelectedAbility);
                
                HighlightCells(ScannedCells);
            }
            else
            {
                _turnManager.ProcessPlayerAction(ScannedCells, cellViewModel.Cell);
                HighlightCells(null);
                SelectedUnit = null;
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
        (int x, int y) cellPosition = cellViewModel.Cell.Position;
        var cell = GameBoard[cellPosition.x, cellPosition.y];
        Console.WriteLine("GBVM sees the click");
        Console.WriteLine($"{cellViewModel.Cell.Icon}");
        IUnit? unitVm = cellViewModel.Cell.Content as IUnit;
        IUnit? unitGb = cell.Content as IUnit;
        if (unitVm != null && unitGb != null)
        {
            Console.WriteLine($"What cellVM knows: {unitVm} - {unitVm.CurrentTurnPhase}");
        }
    }

    private void UpdateGameBoard()
    {
        foreach (var cell in Cells)
        {
            cell.UpdateViewModel(); 
        }
    }

    private (int, int) FetchCoordinates(CellViewModel cellViewModel)
    {
        return cellViewModel.Cell.Position;
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