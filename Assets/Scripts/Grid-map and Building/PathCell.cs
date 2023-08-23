using UnityEngine;

public class PathCell
{
    public ushort bestCost;
    public GridDirection bestDirection;

    public byte cost = 1;
    public Vector3 worldPosition;
    public int x, y;


    public PathCell(int x, int y, Vector3 worldPosition)
    {
        this.x = x;
        this.y = y;
        this.worldPosition = worldPosition;

        bestCost = ushort.MaxValue;
        bestDirection = GridDirection.None;
    }

    public void IncreaseCost(int num)
    {
        if (cost == byte.MaxValue) return;
        if (num + cost >= 255) cost = byte.MaxValue;
        else
            cost += (byte)num;
    }


    public override string ToString()
    {
        return x + "," + y;
    }
}