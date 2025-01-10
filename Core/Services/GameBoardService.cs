namespace BoH.Services;

using BoH.Interfaces;
using BoH.Models;
using System.Threading.Tasks;

public class GameBoardService : IGameBoardService
{
    public IGameBoard GenerateGameBoard(int width, int length)
    {
        // TODO случайная герерация препятствий
        var gameBoard = new GameBoard(width, length);
        return gameBoard;
    }

    public void AddObjectToGameBoard(Object? obj, ICell cell)
    {
        if(obj == null) return;
        cell.Content = obj;
    }

    public void RemoveObjectFromGameBoard(Object? obj, ICell cell)
    {
        if(obj == null) return;
        cell.Content = null;
    }

    public async Task SaveGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }

    public async Task LoadGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }

    public async Task DeleteGameBoardAsync()
    {
        // TODO
        await Task.Delay(0);
    }
}
