using System;
using UnityEngine;
public  class Global:MonoBehaviour
{
    public static int BUILDING_MASK;
    public static int UNIT_MASK;
    public static int Tile_MASK;

    private void Awake()
    {
        BUILDING_MASK = LayerMask.GetMask("Building");
        UNIT_MASK = LayerMask.GetMask("Unit");
        Tile_MASK = LayerMask.GetMask("Tile");

    }
}