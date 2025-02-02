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
    /// <exception cref="OutOfMemoryException"/> 
    // Юниты появляются в диаметрально противоположных углах
    public IGameBoard GenerateGameBoard(int width, int length, Dictionary<string, List<IUnit>> teams)
    {
        var gameBoard = new GameBoard(width, length);
        return gameBoard;
        /*
        Random rnd = new Random();
        var gameBoard = new GameBoard(width, length);
        List<string> teamNames = teams.Keys.ToList();
        List<IIconHolder> teamOne = teams[teamNames[0]];
        List<IUnit> teamTwo = teams[teamNames[1]];

        if (teamOne.Count > gameBoard.Width - 1 || teamTwo.Count > gameBoard.Width - 1)
            throw new OutOfMemoryException("Кол-во юнитов превышает длину поля");

        int x = 0;
        foreach (var unit in teamOne)
        {
            gameBoard[x, width - 1].Content = unit;
            ++x;
        }

        x = length - 1;
        foreach (var unit in teamTwo)
        {
            gameBoard[x, 0].Content = unit;
            --x;
        }

        int obstacleCount = rnd.Next(0, gameBoard.Width);
        int failedPlacements = 0;
        while (obstacleCount != 0)
        {
            x = rnd.Next(0, gameBoard.Width);
            int y = rnd.Next(0, gameBoard.Height);
            if (gameBoard[x, y].Content != null)
            {
                ++failedPlacements;
                if (failedPlacements == 3)
                    break;
            }
            else
            {
                gameBoard[x, y].Content = new Obstacle();
                --obstacleCount;
            }
        }

        return gameBoard;
        */
    }

    /// <inheritdoc/>
    public void AddObjectToGameBoard(IIconHolder? obj, ICell cell)
    {
        if (obj == null) return;
        cell.Content = obj;
    }

    /// <inheritdoc/>
    public void RemoveObjectFromGameBoard(IIconHolder? obj, ICell cell)
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
