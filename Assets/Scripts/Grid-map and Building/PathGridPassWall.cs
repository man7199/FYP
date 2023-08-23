using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathGridPasswall : PathGrid
{
    public static byte[,] costGridArray;


    public PathGridPasswall(int width, int height, float cellSize, Vector3 originPosition, Vector3 destination,bool init)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        worldOffset = new Vector3(cellSize / 2, 0, cellSize / 2);
        gridArray = new PathCell[width, height];


        for (var x = 0; x < width; x++)
        for (var z = 0; z < height; z++)
            gridArray[x, z] = new PathCell(x, z, GetWorldPosition(x, z, true));

        if (costGridArray == null || init)
        {
            InitializeCostGridArray();
        }


        destinationCell = GetCell(destination);


        Computation();
    }

    public void InitializeCostGridArray()
    {
        costGridArray = new byte[width, height];

        var cellHalfExtents = Vector3.one * ((cellSize + 0.0001f) / 2);
        var terrainMask = LayerMask.GetMask("Tile", "Building", "Membrane");
        var tile = LayerMask.NameToLayer("Tile");
        var building = LayerMask.NameToLayer("Building");
        var membrane = LayerMask.NameToLayer("Membrane");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var obstacles = Physics.OverlapBox(GetWorldPosition(i, j, true), cellHalfExtents,
                    Quaternion.identity, terrainMask);
                foreach (var col in obstacles)
                    if (col.gameObject.layer == tile)
                    {
                        if (costGridArray[i, j] < col.GetComponent<Tile>().pathCost)
                        {
                            costGridArray[i, j] = col.GetComponent<Tile>().pathCost;
                        }
                    }
                    else if (col.gameObject.layer == building)
                        costGridArray[i, j] += byte.MaxValue;
                    else if (col.gameObject.layer == membrane)
                        costGridArray[i, j] = 1;
            }
        }
    }


    public override void Computation()
    {
        Thread myNewThread = new Thread(() => HeavyComputation(destinationCell));
        myNewThread.Start();
    }


    public Vector3 GetWorldPosition(int x, int z, bool offset = false)
    {
        if (offset) return new Vector3(x, 0, z) * cellSize + originPosition + worldOffset;
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public bool IsInside(Vector3 position)
    {
        GetGridPosition(position, out var x, out var z);
        if (x < 0 || z < 0 || x > width - 1 || z > height - 1)
            return false;
        return true;
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        z = Mathf.FloorToInt((worldPosition.z - originPosition.z) / cellSize);
    }

    public void SetGridObject(int x, int z, PathCell value)
    {
        if (x >= 0 && x < width && z >= 0 && z < height) gridArray[x, z] = value;
    }

    public void SetGridObject(Vector3 worldPosition, PathCell value)
    {
        GetGridPosition(worldPosition, out var x, out var z);
        SetGridObject(x, z, value);
    }

    public PathCell GetCell(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
            return gridArray[x, z];
        return default;
    }

    public override PathCell GetCell(Vector3 worldPosition)
    {
        GetGridPosition(worldPosition, out var x, out var z);
        return GetCell(x, z);
    }

    private void HeavyComputation(PathCell destinationCell)
    {
        CreateIntegrationField(destinationCell);
        CreateFlowField();
    }


    private void CreateIntegrationField(PathCell _destinationCell)
    {
        destinationCell = _destinationCell;
        if (costGridArray[destinationCell.x, destinationCell.y] == byte.MaxValue)
        {
            Vector2Int position = FindClosestReachableCell(new Vector2Int(destinationCell.x, destinationCell.y));
            destinationCell = gridArray[position.x, position.y];
        }

        destinationCell.bestCost = 0;

        var cellsToCheck = new Queue<PathCell>();
        cellsToCheck.Enqueue(destinationCell);
        while (cellsToCheck.Count > 0)
        {
            var cell = cellsToCheck.Dequeue();
            var neighbors = GetNeightbors(new Vector2Int(cell.x, cell.y), GridDirection.BaseDirections);
            foreach (var neighbor in neighbors)
            {
                if (costGridArray[neighbor.x, neighbor.y] == byte.MaxValue) continue;
                if (costGridArray[neighbor.x, neighbor.y] + cell.bestCost < neighbor.bestCost)
                {
                    neighbor.bestCost = (ushort)(costGridArray[neighbor.x, neighbor.y] + cell.bestCost);
                    cellsToCheck.Enqueue(neighbor);
                }
            }
        }
    }

    private Vector2Int FindClosestReachableCell(Vector2Int position)
    {
        int depth = 1;
        while (true)
        {
            // Debug.Log(depth);
            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    if (i + j == depth)
                    {
                        if (position.x + i < width && position.y + j < height)
                        {
                            if (costGridArray[position.x + i, position.y + j] != byte.MaxValue)
                            {
                                return new Vector2Int(position.x + i, position.y + j);
                            }
                        }

                        if (position.x + i < width && position.y - j >= 0)
                        {
                            if (costGridArray[position.x + i, position.y - j] != byte.MaxValue)
                            {
                                return new Vector2Int(position.x + i, position.y - j);
                            }
                        }

                        if (position.x - i >= 0 && position.y + j < height)
                        {
                            if (costGridArray[position.x - i, position.y + j] != byte.MaxValue)
                            {
                                return new Vector2Int(position.x - i, position.y + j);
                            }
                        }

                        if (position.x - i >= 0 && position.y - j >= 0)
                        {
                            if (costGridArray[position.x - i, position.y - j] != byte.MaxValue)
                            {
                                return new Vector2Int(position.x - i, position.y - j);
                            }
                        }
                    }
                }
            }

            depth++;
        }
    }

    private void CreateFlowField()
    {
        foreach (var cell in gridArray)
        {
            var neighbors = GetNeightbors(new Vector2Int(cell.x, cell.y), GridDirection.AllDirections);
            int bestCost = cell.bestCost;
            foreach (var neighbor in neighbors)
                if (neighbor.bestCost < bestCost)
                {
                    bestCost = neighbor.bestCost;
                    var direction = new Vector2Int(neighbor.x - cell.x, neighbor.y - cell.y);
                    cell.bestDirection = GridDirection.convertVectorToDirection(direction);
                }
        }
    }

    private List<PathCell> GetNeightbors(Vector2Int index, List<GridDirection> directions)
    {
        var neightborCells = new List<PathCell>();
        foreach (Vector2Int direction in directions)
        {
            var finalIndex = index + direction;
            if (finalIndex.x < 0 || finalIndex.x >= width || finalIndex.y < 0 || finalIndex.y >= height) continue;

            neightborCells.Add(gridArray[finalIndex.x, finalIndex.y]);
        }

        return neightborCells;
    }

    public override byte[,] GetCostGridArray()
    {
        return costGridArray;
    }
}