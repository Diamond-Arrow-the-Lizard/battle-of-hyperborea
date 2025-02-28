using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Media;
using BoH.Interfaces;
using BoH.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoH.GUI.ViewModels;


public partial class GameBoardViewModel : ViewModelBase
{
    [ObservableProperty] private GameBoard _gameBoard;
    [ObservableProperty] private IPlayer[] _players;
    [ObservableProperty] private ObservableCollection<CellViewModel> _cells = [];
    [ObservableProperty] private int _width;
    [ObservableProperty] private int _height;
    [ObservableProperty] private List<ICell> _scannedCells = [];
    [ObservableProperty] private IUnit? _selectedUnit = null;
    [ObservableProperty] private TurnPhase? _currentTurnPhase = null;
    [ObservableProperty] private bool? _isTurnEnded = null;
    [ObservableProperty] private IAbility? _selectedAbility = null;
    public event Action<IUnit>? OnUnitSelectedForAbilities;
    
    private readonly ITurnManager _turnManager;
    private readonly IGameController _gameController;
    private int _currentPlayerIndex = 0;


    public GameBoardViewModel(IGameBoard gameBoard, 
        ITurnManager turnManager, 
        IGameController gameController,
        IPlayer[] players)
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
        _gameController = gameController ?? throw new ArgumentNullException(nameof(gameController));
        _players = players ?? throw new ArgumentNullException(nameof(players));
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
                if(SelectedUnit != null) OnUnitSelectedForAbilities?.Invoke(SelectedUnit);
                
                CurrentTurnPhase = SelectedUnit?.CurrentTurnPhase;
                ScannedCells =
                    _turnManager.ProcessScanner(SelectedUnit?.CurrentTurnPhase != TurnPhase.Action ? null : SelectedAbility);
                
                HighlightCells(ScannedCells);
            }
            else
            {
                _turnManager.ProcessPlayerAction(ScannedCells, cellViewModel.Cell, 
                    SelectedUnit?.CurrentTurnPhase == TurnPhase.Movement ? null : SelectedAbility);
                HighlightCells(null);
                SelectedUnit = null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            HighlightCells(null);
        }
        
        finally
        {
            UpdateGameBoard();
            if (_gameController.CheckVictoryCondition(Players))
            {
                _gameController.EndGame();
            }
            if (_gameController.CheckForTurnEnd(Players[_currentPlayerIndex]))
            {
                _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Length;
                _turnManager.EndTurn();
                _turnManager.StartNewRound(_turnManager.CurrentPlayer);
            }
        }
    }

    private void Debug_GbvmSeesClick(CellViewModel cellViewModel)
    {
        (int x, int y) cellPosition = cellViewModel.Cell.Position;
        var cell = GameBoard[cellPosition.x, cellPosition.y];
        Console.WriteLine("GBVM sees the click");
        Console.WriteLine($"{cellViewModel.Cell.Icon}");
        IUnit? unitVm = cellViewModel.Cell.Content as IUnit;
        if (unitVm != null)
        {
            Console.WriteLine($"What cellVM knows: {unitVm} - {unitVm.CurrentTurnPhase}, HP: {unitVm.Hp}, Is dead: {unitVm.IsDead}");
        }
    }

    public void UpdateScan(IAbility ability)
    {
        ScannedCells = _turnManager.ProcessScanner(ability);
        HighlightCells(ScannedCells);
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