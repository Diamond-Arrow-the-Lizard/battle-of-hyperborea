namespace BoH.GameLogic;

using BoH.Interfaces;
using BoH.Services;
using System.Threading.Tasks;

/// <summary>
/// Класс для управления игровым процессом.
/// </summary>
public class GameController : IGameController
{
    private readonly GameBoardService _boardService;

    public GameController(GameBoardService boardService)
    {
        _boardService = boardService;
    }

    /// <inheritdoc/>
    public async Task StartGame()
    {
        await Task.Delay(0);
    }

    /// <inheritdoc/>
    public void NextTurn()
    {

    }

    /// <inheritdoc/>
    public bool CheckVictoryCondition()
    {
        return false; 
    }

    /// <inheritdoc/>
    public async Task EndGame()
    {
        await Task.Delay(0);
    }
}