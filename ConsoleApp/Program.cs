using System;
using EntityClass;
using TileClass;
using AbilityClass;
using ScannerClass;
using MovementClass;

namespace Main;
public class Program {

    public static void Main(string[] args) {

        Tile[ , ] Field = new Tile[5 , 5];

        for(int i = 0; i < 5; i++) {
            for(int j = 0; j < 5; j++) {
                Field[i, j] = new Tile(i, j);
                /*
                   Console.WriteLine(Field[i, j].x);
                   Console.WriteLine(Field[i, j].y);
                   Console.WriteLine(Field[i, j].tileType);
                   */
            }
        }

        Ability Ab = new Ability();
        Entity Guy = new Entity(1, 3, 100, 100, 3, false, Ab);
        Entity Bandit = new Entity(3, 2, 100, 100, 3, false, Ab);

        Field[1, 3].tileType = TileTypes.Entity;
        Field[3, 2].tileType = TileTypes.Entity;


        string[,] GraphicField = new string[5, 5];
        for(int i = 0; i < 5; i++) {
            for(int j = 0; j < 5; j++) {
                TileTypes Switcher = Field[i, j].tileType; 
                switch (Switcher) {

                    case TileTypes.Empty:
                        GraphicField[i, j] = "O";
                        break;
                    case TileTypes.Obstacle:
                        GraphicField[i, j] = "X";
                        break;
                    case TileTypes.Entity:
                        GraphicField[i, j] = "T";
                        break;
                    default:
                        Console.WriteLine("What?");
                        break;
                }

            }


        }

        for(int i = 0; i < 5; i++) {
            Console.WriteLine();
            for(int j = 0; j < 5; j++) {
                Console.Write(GraphicField[i, j]);
            }
        }
        Console.WriteLine();



        string[,] ScannedField = GraphicField;

        List<int[]> FOV = Scanner.ScanTiles(Bandit.x, Bandit.y, 3);
        /*
        foreach(var item in FOV) {
            Console.WriteLine();
            foreach(var i in item) {
                Console.Write($"{i} ");
            }
        }
        Console.WriteLine();
        */

        foreach(var item in FOV) {
            if(item[0] > 4 || item[1] > 4) continue;
            int x = item[0];
            int y = item[1];
            TileTypes Switcher = Field[x, y].tileType; 
            switch (Switcher) {

                case TileTypes.Empty:
                    GraphicField[x, y] = "W";
                    break;
                case TileTypes.Obstacle:
                    GraphicField[x, y] = "X";
                    break;
                case TileTypes.Entity:
                    GraphicField[x, y] = "!";
                    break;
                default:
                    Console.WriteLine("What?");
                    break;
            }
            
        }

        GraphicField[Bandit.x, Bandit.y] = "T";
        for(int i = 0; i < 5; i++) {
            Console.WriteLine();
            for(int j = 0; j < 5; j++) {
                Console.Write(ScannedField[i, j]);
            }
        }
        Console.WriteLine();

        Bandit.Move(Field, 1, 2);
        for(int i = 0; i < 5; i++) {
            for(int j = 0; j < 5; j++) {
                TileTypes Switcher = Field[i, j].tileType; 
                switch (Switcher) {

                    case TileTypes.Empty:
                        GraphicField[i, j] = "O";
                        break;
                    case TileTypes.Obstacle:
                        GraphicField[i, j] = "X";
                        break;
                    case TileTypes.Entity:
                        GraphicField[i, j] = "T";
                        break;
                    default:
                        Console.WriteLine("What?");
                        break;
                }

            }

        }
        for(int i = 0; i < 5; i++) {
            Console.WriteLine();
            for(int j = 0; j < 5; j++) {
                Console.Write(GraphicField[i, j]);
            }
        }
        Console.WriteLine();



    }
    
}
