using System;
using EntityClass;
using TileClass;
using AbilityClass;

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
        Entity Bandit = new Entity(1, 4, 100, 100, 3, false, Ab);

        Field[0, 2].tileType = TileTypes.Entity;
        Field[0, 3].tileType = TileTypes.Entity;


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

    }

}
