using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class BuildingController : NetworkBehaviour
{
    public static BuildingController Instance;

    public Building[] buildings;

    public enum SetBuilding
    {
        CellBase,
        BoneMarrow2,
        BoneMarrow3,
        Thymus1,
        Thymus2,
        CellMembrane,
        Cellwall,
        RobotBase,
        SolarPanel,
        RepairFactory,
        Turret,
        Cannon,
        HealthCenter
    }

    private int currentBuildingIndex;
    private Direction currentDirection = Direction.Down;
    public TileGrid tileGrid;
    public BuildingUI UI;
    public event EventHandler OnBuildingChange;

    public enum Direction
    {
        Down,
        Right,
        Top,
        Left
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There should be only one BuildingController.");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UI = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
        tileGrid = TileSystem.Instance.grid;
    }


    public void SwitchBuilding()
    {
        currentBuildingIndex++;
        currentBuildingIndex %= buildings.Length;
        OnBuildingChange?.Invoke(this, EventArgs.Empty);
    }

    public void SwitchBuilding(int x)
    {
        currentBuildingIndex = x;
    }

    public void Rotate()
    {
        currentDirection = GetNextDirection(currentDirection);
        OnBuildingChange?.Invoke(this, EventArgs.Empty);
    }

    public void ResetRotation()
    {
        currentDirection = Direction.Down;
        OnBuildingChange?.Invoke(this, EventArgs.Empty);
    }

    public void SelectBuildingCommand(Vector3 worldPosition)
    {
        // TODO: select building belongs to other
        int mask = LayerMask.GetMask("Building");
        Ray ray = Camera.main.ScreenPointToRay(worldPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 50000.0f, mask))
        {
            Building building = hit.transform.GetComponentInParent<Building>();

            UI.changeUI(building);
        }
        else
        {
            // clear UI
            UI.changeUI();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPCReplaceBuildingCommand(Vector3 worldPosition, NetworkPrefabRef buildingPrefab,Vector3 instantiatePosition)
    {
        PlayerRef playerRef = GetBuilding(worldPosition).Owner;
        DestroyBuilding(worldPosition);

        IEnumerator place()
        {
            yield return new WaitForSeconds(0.1f);
            RpcPlacingBuilding(buildingPrefab, worldPosition, playerRef,instantiatePosition);
        }

        StartCoroutine(place());
    }

    public bool CheckIsPositionValid(Vector3 worldPosition, Building building, Unit builder = null)
    {
        if (!tileGrid.IsInside(worldPosition))
        {
            Debug.LogError("The position is outside the grid.");
            return false;
        }

        if (building == null)
        {
            building = buildings[currentBuildingIndex];
        }

        tileGrid.GetGridPosition(worldPosition, out var xPos, out var zPos);
        int buildingWidth, buildingHeight;
        if (currentDirection == Direction.Left || currentDirection == Direction.Right)
        {
            buildingWidth = building.height;
            buildingHeight = building.width;
        }
        else
        {
            buildingWidth = building.width;
            buildingHeight = building.height;
        }

        //Check Boundary
        if (xPos + buildingWidth > tileGrid.GetWidth() || zPos + buildingHeight > tileGrid.GetHeight())
        {
            Debug.LogError("This location is invalid");
            return false;
        }

        //Check Canbuild
        var cellList = GetCellList(building, new Vector2Int(xPos, zPos), currentDirection);
        foreach (var cell in cellList)
        {
            if (!cell.CanBuild())
            {
                Debug.LogError("This location is invalid");
                return false;
            }
        }

        //Check whether any units are inside
        if (IsUnitInside(cellList, builder))
        {
            Debug.LogError("There are units blocking the position.");
            return false;
        }

        return true;
    }


    public bool PlacingBuildingCommand(Vector3 worldPosition, Building building, Unit builder = null)
    {
        if (building == null)
        {
            building = buildings[currentBuildingIndex];
        }

        if (CheckIsPositionValid(worldPosition, building, builder))
        {
            CalculateTransform(worldPosition, out Vector3 instantiatePosition,building);
            RpcPlacingBuilding(building.prefabRef, worldPosition, Runner.LocalPlayer,instantiatePosition, (int)currentDirection);
            if (builder != null)
            {
                RpcDestroyUnit(builder);
            }

            return true;
        }

        return false;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcDestroyUnit(Unit unit)
    {
        Runner.Despawn(unit.Object);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcPlacingBuilding(NetworkPrefabRef buildingPrefab, Vector3 position, PlayerRef playerRef,Vector3 instantiatePosition,
        int _direction = (int)Direction.Down)
    {
        Direction direction = (Direction)_direction;
        tileGrid.GetGridPosition(position, out var xPos, out var zPos);
        NetworkObject buildingInstance = Runner.Spawn(buildingPrefab, instantiatePosition,
            Quaternion.Euler(0, BuildingController.GetDirectionAngle(direction), 0), playerRef);
        Building building = buildingInstance.GetComponent<Building>();
        building.direction = direction;
        building.x = xPos;
        building.y = zPos;
        building.Owner = playerRef;
        Unit[] units = building.GetComponentsInChildren<Unit>();
        foreach (Unit unit in units)
        {
            unit.Owner = playerRef;
        }

        int buildingWidth, buildingHeight;
        if (direction == Direction.Left || direction == Direction.Right)
        {
            buildingWidth = building.height;
            buildingHeight = building.width;
        }
        else
        {
            buildingWidth = building.width;
            buildingHeight = building.height;
        }

        var cellList = GetCellList(building, new Vector2Int(xPos, zPos), direction);
        foreach (var cell in cellList) cell.SetBuilding(building);

        // update the costGrid in pathfinding system
        Vector2Int gridPosition = tileGrid.GetGridPosition(position);
        Vector2Int size = new Vector2Int(buildingWidth, buildingHeight);
        PathFindingSystem.Instance.UpdateCostGrid(gridPosition, size, byte.MaxValue, building.isMembrane);

        // recompute all pathfindings, very expensive
        UnitController.Instance.RecomputeAllPathGrid();
    }


    public bool DestroyBuilding(Vector3 worldPosition)
    {
        if (!tileGrid.IsInside(worldPosition))
        {
            Debug.Log("The position is outside the grid.");
            return false;
        }

        var building = tileGrid.GetGridObject(worldPosition).GetBuilding();

        return DestroyBuilding(building);
    }

    public bool DestroyBuilding(Building building)
    {
        if (building != null)
        {
            // remove link from tile system to the building
            var cellList =
                GetCellList(building, new Vector2Int(building.x, building.y), building.direction);
            foreach (var cell in cellList) cell.ClearBuilding();

            // update the costGrid in pathfinding system
            Vector2Int gridPosition = new Vector2Int(building.x, building.y);
            int buildingHeight = building.height;
            int buildingWidth = building.width;
            if (building.direction == Direction.Left || building.direction == Direction.Right)
            {
                buildingWidth = buildingHeight;
                buildingHeight = building.width;
            }

            Vector2Int size = new Vector2Int(buildingWidth, buildingHeight);
            PathFindingSystem.Instance.UpdateCostGrid(gridPosition, size, 1);

            // recompute all pathfindings
            UnitController.Instance.RecomputeAllPathGrid();
            building.OnDestroy();

            Destroy(building.gameObject);


            return true;
        }
        else
        {
            Debug.Log("No Building To Remove!");
            return false;
        }
    }

    public Building GetBuilding(Vector3 worldPosition)
    {
        return tileGrid.GetGridObject(worldPosition).GetBuilding();
    }


    public Building GetCurrentBuilding()
    {
        return buildings[currentBuildingIndex];
    }

    public Direction GetDirection()
    {
        return currentDirection;
    }

    public void CalculateTransform(Vector3 worldPosition, out Vector3 position, Building currentBuilding=null)
    {
        tileGrid.GetGridPosition(worldPosition, out var x, out var z);
        if (currentBuilding == null)
        {
            buildings[currentBuildingIndex].TryGetComponent(out currentBuilding);
        }
        GetTransformOffset(currentBuilding, currentDirection, out var xOffset, out var zOffset);
        position = tileGrid.GetWorldPosition(x, z) + new Vector3(xOffset, 0, zOffset) * tileGrid.GetCellSize();
    }

    private bool IsUnitInside(List<TileCell> tileCells, Unit builder = null)
    {
        var cellHalfExtents = Vector3.one * (tileGrid.cellSize / 2);
        var terrainMask = LayerMask.GetMask("Unit");

        foreach (var cell in tileCells)
        {
            var obstacles =
                Physics.OverlapBox(cell.worldPosition, cellHalfExtents, Quaternion.identity,
                    terrainMask);
            foreach (var col in obstacles)
                // detect unit inside
            {
                Unit unit = col.GetComponent<Unit>();
                if (unit.GetType() != typeof(BuildingUnit) && unit != builder)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static Direction GetNextDirection(Direction direction)
    {
        switch (direction)
        {
            default:
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Top;
            case Direction.Top:
                return Direction.Right;
            case Direction.Right:
                return Direction.Down;
        }
    }

    public static int GetDirectionAngle(Direction direction)
    {
        switch (direction)
        {
            default:
            case Direction.Down:
                return 0;
            case Direction.Left:
                return 90;
            case Direction.Top:
                return 180;
            case Direction.Right:
                return 270;
        }
    }

    private static void GetTransformOffset(Building building, Direction direction, out int x, out int z)
    {
        switch (direction)
        {
            default:
            case Direction.Down:
                x = 0;
                z = 0;
                break;
            case Direction.Left:
                x = 0;
                z = building.width;
                break;
            case Direction.Top:
                x = building.width;
                z = building.height;
                break;
            case Direction.Right:
                x = building.height;
                z = 0;
                break;
        }
    }


    private List<TileCell> GetCellList(Building building, Vector2Int position, Direction direction)
    {
        var list = new List<TileCell>();
        int buildingWidth, buildingHeight;
        if (direction == Direction.Left || direction == Direction.Right)
        {
            buildingWidth = building.height;
            buildingHeight = building.width;
        }
        else
        {
            buildingWidth = building.width;
            buildingHeight = building.height;
        }

        for (var x = position.x; x < position.x + buildingWidth; x++)
        for (var z = position.y; z < position.y + buildingHeight; z++)
            list.Add(tileGrid.GetGridObject(x, z));

        return list;
    }
}