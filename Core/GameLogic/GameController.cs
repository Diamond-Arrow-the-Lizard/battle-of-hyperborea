namespace BoH.GameLogic;

using BoH.Interfaces;
using BoH.Models;

public class GameController : IGameController
{
    private readonly IGameBoardService _gameBoardService;

    public GameController(IGameBoardService gameBoardService)
    {
        _gameBoardService = gameBoardService ?? throw new ArgumentNullException(nameof(gameBoardService));
    }

    /// <inheritdoc/>
    public IGameBoard StartGame(int width, int length, IPlayer[] players)
    {
        IGameBoard gameBoard = new GameBoard(width, length);

        return gameBoard;
    }

    /// <inheritdoc/>
    public bool CheckVictoryCondition(IPlayer[] players)
    {
        foreach (var p in players)
        {
            if(!p.HasAliveUnits) return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public async Task EndGame()
    {
        await _gameBoardService.DeleteGameBoardAsync();
    }
}