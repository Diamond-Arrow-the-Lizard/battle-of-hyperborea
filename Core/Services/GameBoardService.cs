namespace BoH.Services;

using BoH.Interfaces;
using BoH.Models;
using System.Threading.Tasks;

/// <summary>
/// Сервис для управления игровым полем.
/// </summary>
public class GameBoardService : IGameBoardService
{
    /// <inheritdoc/>
    public IGameBoard GenerateGameBoard(int width, int length)
    {
        // TODO случайная герерация препятствий
        var gameBoard = new GameBoard(width, length);
        return gameBoard;
    }

    /// <inheritdoc/>
    public void AddObjectToGameBoard(object? obj, ICell cell)
    {
        if (obj == null) return;
        cell.Content = obj;
    }

    /// <inheritdoc/>
    public void RemoveObjectFromGameBoard(object? obj, ICell cell)
    {
        if (obj == null) return;
        cell.Content = null;
    }

    /// <inheritdoc/>
    public async Task SaveGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }

    /// <inheritdoc/>
    public async Task LoadGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }

    /// <inheritdoc/>
    public async Task DeleteGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }
}
