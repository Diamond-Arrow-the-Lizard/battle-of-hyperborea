namespace ScannerClass;

public interface IScanner {
/*    
    public int x{set; get;}
    public int y{set; get;}
    public int Range{set; get;}
    public List<int[,]> SeenCoordinates{set; get;}
*/
    public abstract static List<int[]> ScanTiles(int x, int y, int Range);

}
