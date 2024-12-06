namespace Tile;
/// <summary>
///  Interface for a distinct tile
/// </summary>
public interface ITile {
    public int x {set; get;}
    public int y {set; get;}
    public TileTypes tileType {set; get;}
}
