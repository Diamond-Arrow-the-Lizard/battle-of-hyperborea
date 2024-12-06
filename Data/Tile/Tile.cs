namespace Tile;

///<summary>
/// ITile interface realisation
/// </summary>
public class Tile : ITile {
    public int x {set; get;}
    public int y {set; get;}

    private TileTypes _tile_type = TileTypes.Empty;
    public TileTypes tileType {
        set { _tile_type = value; }
        get { return _tile_type; }
    }

    ///<summary>
    /// Constructor for an empty tile
    /// </summary>
    public Tile(int x, int y) {
        this.x = x;
        this.y = y;
    }

    ///<summary>
    /// Constructor for a tile to specify what it currently has
    /// </summary>
    public Tile(int x, int y, TileTypes tileType) {
        this.x = x;
        this.y = y;
        this.tileType = tileType;
    }
}
