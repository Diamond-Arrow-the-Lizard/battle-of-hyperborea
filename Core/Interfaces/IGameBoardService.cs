namespace BoH.Interfaces;


public interface IGameBoardService
{
    IGameBoard GenerateGameBoard(int width, int length);

    void AddObjectToGameBoard(Object? obj, ICell cell);

    void RemoveObjectFromGameBoard(Object? obj, ICell cell);

    Task SaveGameBoardAsync();

    Task LoadGameBoardAsync();

    Task DeleteGameBoardAsync();

}