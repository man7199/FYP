using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridDirection
{
    public static readonly GridDirection None = new(0, 0);
    public static readonly GridDirection North = new(0, 1);
    public static readonly GridDirection South = new(0, -1);
    public static readonly GridDirection East = new(1, 0);
    public static readonly GridDirection West = new(-1, 0);
    public static readonly GridDirection NorthEast = new(1, 1);
    public static readonly GridDirection NorthWest = new(-1, 1);
    public static readonly GridDirection SouthEast = new(1, -1);
    public static readonly GridDirection SouthWest = new(-1, -1);

    public static readonly List<GridDirection> BaseDirections = new()
    {
        North,
        South,
        East,
        West
    };

    public static readonly List<GridDirection> AllDirections = new()
    {
        None,
        North,
        South,
        East,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    };

    public readonly Vector2Int vector;

    private GridDirection(int x, int y)
    {
        vector = new Vector2Int(x, y);
    }

    public static implicit operator Vector2Int(GridDirection direction)
    {
        return direction.vector;
    }

    public static GridDirection convertVectorToDirection(Vector2Int vector)
    {
        return AllDirections.DefaultIfEmpty(None).FirstOrDefault(Direction => Direction == vector);
    }
}