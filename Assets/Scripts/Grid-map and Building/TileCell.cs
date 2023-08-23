using UnityEngine;

public class TileCell
{
    public readonly Vector3 worldPosition;
    public Building building;
    public Tile tile;

    private TileGrid tileGrid;
    public int x, y;

    public TileCell(TileGrid tileGrid, int x, int y, Vector3 worldPosition, Tile tile)
    {
        this.tileGrid = tileGrid;
        this.x = x;
        this.y = y;
        this.worldPosition = worldPosition;
        this.tile = tile;
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }

    public void SetBuilding(Building building)
    {
        this.building = building;
    }

    public bool CanBuild()
    {
        return building == null && tile.buildable;
    }

    public void ClearBuilding()
    {
        if (building == null) Debug.Log("Try to clear building of a cell, when the cell is empty!");
        building = null;
    }

    public Building GetBuilding()
    {
        return building;
    }
}