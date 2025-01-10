namespace BoH.CLI;

using BoH.Interfaces;
using BoH.Models;
using BoH.Units;

public class Program
{
    public static void Main(string[] args)
    {
        var board = new GameBoard(5, 5);

        // Заполнение игрового поля
        board[0, 0].Content = new Obstacle();
        board[1, 1].Content = new BaseUnit('A'); // Юнит с иконкой 'A'
        board[1, 4].Content = new BaseUnit(); // Юнит с иконкой 'T'
        board[2, 2].Content = null; // Пустая клетка

        // Рендеринг игрового поля
        GameBoardRenderer.DrawBoard(board);

    }
}