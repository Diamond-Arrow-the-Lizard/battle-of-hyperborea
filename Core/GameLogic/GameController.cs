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
    public bool CheckForTurnEnd(IPlayer player)
    {
        foreach (var i in player.Units)
        {
            if (i.CurrentTurnPhase != TurnPhase.End)
                return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public bool CheckVictoryCondition(IPlayer[] players)
    {
        foreach (var p in players)
        {
            if (!p.HasAliveUnits) return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public async Task EndGame()
    {
        await _gameBoardService.DeleteGameBoardAsync();
    }
}