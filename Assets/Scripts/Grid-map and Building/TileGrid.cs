using System;
using UnityEngine;

public class TileGrid
{
    public readonly float cellSize;
    public readonly TileCell[,] gridArray;
    public readonly int height;
    public readonly Vector3 originPosition;
    public readonly int width;
    public readonly Vector3 worldOffset;

    public TileGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        worldOffset = new Vector3(cellSize / 2, 0, cellSize / 2);
        gridArray = new TileCell[width, height];

        var cellHalfExtents = Vector3.one * (cellSize / 2 - 0.5f);
        var terrainMask = LayerMask.GetMask("Tile");

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            var obstacles =
                Physics.OverlapBox(GetWorldPosition(x, y, true), cellHalfExtents, Quaternion.identity,
                    terrainMask);
            Tile thisTile = null;
            foreach (var col in obstacles)
            {
                var tile = col.GetComponent<Tile>();
                if (tile != null) thisTile = tile;
            }

            gridArray[x, y] = new TileCell(this, x, y, GetWorldPosition(x, y) + worldOffset, thisTile);
        }
    }




    public Vector3 GetWorldPosition(int x, int y, bool offset = false)
    {
        if (offset) return new Vector3(x, 0, y) * cellSize + originPosition + worldOffset;

        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public bool IsInside(Vector3 position)
    {
        GetGridPosition(position, out var x, out var z);
        if (x < 0 || z < 0 || x > width - 1 || z > height - 1)
            return false;
        return true;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        int z = Mathf.FloorToInt((worldPosition.z - originPosition.z) / cellSize);
        Vector2Int position = new Vector2Int(x,z);
        return position;
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        z = Mathf.FloorToInt((worldPosition.z - originPosition.z) / cellSize);
    }


    public TileCell GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            return gridArray[x, y];
        return null;
    }

    public TileCell GetGridObject(Vector3 worldPosition)
    {
        GetGridPosition(worldPosition, out var x, out var y);
        return GetGridObject(x, y);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }
}