namespace ScannerClass;

public class Scanner : IScanner {
/*
    public int x{set; get;}
    public int y{set; get;}

    private int _range = 1;
    public int Range { 
        set {
            if(value <= 0)
                throw new ("ERROR: range should be more than zero");
            else
                _range = value;
        }
        get {
            return _range;
        }
    }
    
       private List<int[,]> _seen_coordinates = new List<int[,]>();
       public List<int[,]> SeenCoordinates {
       set {
       if(value == null)
       throw new ("ERROR: new value is null");
       else
       _seen_coordinates = value;
       }
       get {
       return _seen_coordinates;
       }

       }
       */   
    
    public static int[] ScanTile(int x, int y) {
        int[] scannedTile = new int[]{x, y}; 
        return scannedTile;
    }

    public static List<int[]> ScanTiles(int x, int y, int Range) {
        List<int[]> SeenCoordinates = new List<int[]>();
        int rangeX = 0;
        int rangeY = 0;

        for(rangeX = 0; rangeX < Range; rangeX++) {
            rangeY = 0;
            while(rangeY != Range) {
                int[] scannedTile = ScanTile(x+rangeX, y+rangeY);
                SeenCoordinates.Add(scannedTile);
                scannedTile = ScanTile(x+rangeX, y-rangeY);
                SeenCoordinates.Add(scannedTile);
                scannedTile = ScanTile(x-rangeX, y+rangeY);
                SeenCoordinates.Add(scannedTile);
                scannedTile = ScanTile(x-rangeX, y-rangeY);
                SeenCoordinates.Add(scannedTile);

                ++rangeY;
            }
        }

        return SeenCoordinates;
    }
}
