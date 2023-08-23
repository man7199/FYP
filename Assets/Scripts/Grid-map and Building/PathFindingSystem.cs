using UnityEngine;

public class PathFindingSystem
{
    public static PathFindingSystem Instance { get; private set; }
    public readonly float cellSize;
    private readonly int height;
    private readonly Vector3 originPosition;
    private readonly int width;


    public PathFindingSystem(int width, int height, float cellSize, Vector3 originPosition)
    {
        Instance = this;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
    }


    public void GetPathGrid(Vector3 position, out PathGridNormal pathGridNormal, out PathGridPasswall pathGridPasswall,bool init=false)
    {
        pathGridNormal = new PathGridNormal(width, height, cellSize, originPosition, position,init);
        pathGridPasswall = new PathGridPasswall(width, height, cellSize, originPosition, position,init);
    }

    public void UpdateCostGrid(Vector2Int tileGridPosition, Vector2Int changeSize, byte value, bool ismembrane = false)
    {
        TileSystem tileSystem = TileSystem.Instance;
        SetupMap setupMap = SetupMap.Instance;
        Vector2Int position = new Vector2Int(tileGridPosition.x * setupMap.nodePerCell,
            tileGridPosition.y * setupMap.nodePerCell);
        Vector2Int size = new Vector2Int(changeSize.x * setupMap.nodePerCell, changeSize.y * setupMap.nodePerCell);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int indexI = position.x + i;
                int indexJ = position.y + j;
                if (indexI >= 0 && indexJ >= 0 && indexI < width && indexJ < height)
                {
                    PathGridNormal.costGridArray[indexI, indexJ] = value;
                    if (!ismembrane)
                    {
                        PathGridPasswall.costGridArray[indexI, indexJ] = value;
                    }
                }
            }
        }
    }
}