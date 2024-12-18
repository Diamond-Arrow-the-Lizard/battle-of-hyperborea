using TileClass;
using EntityClass;

namespace MovementClass;

public static class Movement {

    public static void Move(this Entity MovingEntity, Tile[,] Field, int NewX, int NewY) {
        
        if(Field[NewX, NewY].tileType != TileTypes.Empty) {
            throw new ("ERROR: tile is occupied");
        }

        Field[MovingEntity.x, MovingEntity.y].tileType = TileTypes.Empty;
        Field[NewX, NewY].tileType = TileTypes.Entity;
        
        MovingEntity.x = NewX;
        MovingEntity.y = NewY;
    }
}
