using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class PathGrid
{ 
    protected  float cellSize;
    public PathCell destinationCell;
    public PathCell[,] gridArray;
    public int height;
    protected  Vector3 originPosition;
    public int width;
    public Vector3 worldOffset;
    

    public abstract void Computation();
    public abstract PathCell GetCell(Vector3 worldPosition);
    public abstract byte[,] GetCostGridArray();

}