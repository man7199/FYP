using System.Collections.Generic;

public class UnitGroup
{
    public PathGrid grid1;
    public PathGrid grid2;
    public PathGrid currentGrid;
    public bool patrol;
    public HashSet<Unit> unitSet;
    
    public UnitGroup(PathGrid grid1, PathGrid grid2 = null)
    {
        this.grid1 = grid1;
        currentGrid = grid1;
        if (grid2 != null)
        {
            this.grid2 = grid2;
            patrol = true;
        }

        unitSet = new HashSet<Unit>();
    }

    public UnitGroup(HashSet<Unit> unitSet, PathGrid grid1, PathGrid grid2 = null)
    {
        this.grid1 = grid1;
        currentGrid = grid1;
        if (grid2 != null)
        {
            this.grid2 = grid2;
            patrol = true;
        }

        this.unitSet = unitSet;
    }

    public void AddUnit(Unit unit)
    {
        if (!unitSet.Contains(unit)) unitSet.Add(unit);
    }

    public void AddUnits(HashSet<Unit> units)
    {
        foreach (Unit unit in units)
        {
            unitSet.Add(unit);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        unitSet.Remove(unit);
    }
}