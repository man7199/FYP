using UnityEngine;

public class TileSystem
{
    
    public static TileSystem Instance { get; private set; }
    public readonly TileGrid grid;
    
    public TileSystem(int width, int height, float cellSize, Vector3 originPosition)
    {
        Instance = this;
        // GameObject ghost = new GameObject("Building Ghost", typeof(BuildingGhost));
        grid = new TileGrid(width, height, cellSize, originPosition);
    }

}