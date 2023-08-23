using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UnitController : NetworkBehaviour
{
    public static UnitController Instance;
    public float stoppingFactor;
    public float checkStopPeriod;
    private float recomputeCounter;
    private float stoppingCounter;
    public HashSet<UnitGroup> unitGroups;


    public UnitUI UI; //added


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There should be only one UnitController.");
            return;
        }

        unitGroups = new HashSet<UnitGroup>();
        Instance = this;


        UI = GameObject.Find("UnitUI").GetComponent<UnitUI>(); // added
    }


    private void FixedUpdate()
    {
        HandleUnitMovement();
    }

    public void RecomputeAllPathGrid()
    {
        foreach (var unitGroup in unitGroups) unitGroup.grid1.Computation();
    }


    public void PatrolCommand(Vector3 position1, Vector3 position2)
    {
        Patrol(UnitSelection.Instance.selectedUnits, position1, position2);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCAttackCommand(Unit[] units, Unit target)
    {
        foreach (Unit unit in units)
        {
            unit.target = target.transform;
        }
    }
    
    


    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCMoveCommand(Unit[] units, Vector3 position, bool isForcedMovement)
    {
        var set = new HashSet<Unit>(units);
        MoveUnit(set, position, isForcedMovement, out UnitGroup group1, out UnitGroup group2);
        // Debug.Log(units.Length+position.ToString());
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCMoveCommand(Unit unit, Vector3 position, bool isForcedMovement)
    {
        MoveUnit(unit, position, isForcedMovement);
    }

    public void MoveUnit(Unit unit, Vector3 worldPosition, bool isForcedMovement)
    {
        HashSet<Unit> units = new HashSet<Unit>() { unit };
        MoveUnit(units, worldPosition, isForcedMovement, out UnitGroup group1, out UnitGroup group2);
    }

    public void MoveUnit(HashSet<Unit> units, Vector3 worldPosition, bool isForcedMovement,
        out UnitGroup normalUnitGroup, out UnitGroup passWallUnitGroup)
    {
        PathFindingSystem.Instance.GetPathGrid(worldPosition, out PathGridNormal pathGridNormal,
            out PathGridPasswall pathGridPasswall);
        normalUnitGroup = new UnitGroup(pathGridNormal);
        passWallUnitGroup = new UnitGroup(pathGridPasswall);

        foreach (var unit in units)
        {
            if (unit.unitGroup != null) unit.unitGroup.RemoveUnit(unit);
            if (unit.canPassWall)
            {
                unit.unitGroup = passWallUnitGroup;
                passWallUnitGroup.AddUnit(unit);
            }
            else
            {
                unit.unitGroup = normalUnitGroup;
                normalUnitGroup.AddUnit(unit);
            }

            if (isForcedMovement)
            {
                unit.target = null;
                unit.isForcedMovement = true;
            }
        }

        if (normalUnitGroup.unitSet.Count > 0)
        {
            unitGroups.Add(normalUnitGroup);
        }

        if (passWallUnitGroup.unitSet.Count > 0)
        {
            unitGroups.Add(passWallUnitGroup);
        }
    }

    public void Patrol(Unit unit, Vector3 position1, Vector3 position2)
    {
        HashSet<Unit> units = new HashSet<Unit>() { unit };
        Patrol(units, position1, position2);
    }

    public void Patrol(HashSet<Unit> units, Vector3 position1, Vector3 position2)
    {
        PathGridNormal pathGrid1;
        PathGridPasswall pathGridPasswall1;
        PathFindingSystem.Instance.GetPathGrid(position1, out pathGrid1, out pathGridPasswall1);

        PathGridNormal pathGrid2;
        PathGridPasswall pathGridPasswall2;
        PathFindingSystem.Instance.GetPathGrid(position2, out pathGrid2, out pathGridPasswall2);

        UnitGroup normalUnitGroup = new UnitGroup(pathGrid1, pathGrid2);
        UnitGroup passWallUnitGroup = new UnitGroup(pathGrid1, pathGrid2);

        foreach (var unit in units)
        {
            if (unit.unitGroup != null) unit.unitGroup.RemoveUnit(unit);
            if (unit.canPassWall)
            {
                unit.unitGroup = passWallUnitGroup;
                passWallUnitGroup.AddUnit(unit);
            }
            else
            {
                unit.unitGroup = normalUnitGroup;
                normalUnitGroup.AddUnit(unit);
            }

            unit.target = null;
        }

        if (normalUnitGroup.unitSet.Count > 0)
        {
            unitGroups.Add(normalUnitGroup);
        }

        if (passWallUnitGroup.unitSet.Count > 0)
        {
            unitGroups.Add(passWallUnitGroup);
        }
    }

    private void HandleUnitMovement()
    {
        stoppingCounter += Time.fixedDeltaTime;
        if (unitGroups.Count != 0 && stoppingCounter >= checkStopPeriod)
        {
            stoppingCounter = 0;

            // remove unitgroup if it is empty
            unitGroups.RemoveWhere(group => group.unitSet.Count == 0);

            // check for stopping unitgroup
            foreach (UnitGroup unitGroup in unitGroups)
            {
                var canStop = true;
                var destination = unitGroup.currentGrid.destinationCell.worldPosition;
                var stoppingDistance = stoppingFactor * (float)Math.Sqrt(unitGroup.unitSet.Count * 0.6f * 0.6f) +
                                       PathFindingSystem.Instance.cellSize / 2;
                unitGroup.unitSet.RemoveWhere((unit) =>
                {
                    if (Vector3.Distance(unit.transform.position, unitGroup.currentGrid.destinationCell.worldPosition) <
                        5f && !unitGroup.patrol)
                    {
                        unit.unitGroup = null;
                        return true;
                    }

                    return false;
                });


                foreach (var unit in unitGroup.unitSet)
                {
                    var distance = Vector3.Distance(destination, unit.transform.position);
                    if (distance >= stoppingDistance)
                    {
                        canStop = false;
                        break;
                    }
                }

                //Stop whole group
                if (canStop)
                {
                    //handle patrol group
                    if (unitGroup.patrol)
                    {
                        if (unitGroup.currentGrid == unitGroup.grid1)
                        {
                            unitGroup.currentGrid = unitGroup.grid2;
                        }
                        else
                        {
                            unitGroup.currentGrid = unitGroup.grid1;
                        }
                    }
                    else
                    {
                        foreach (var unit in unitGroup.unitSet)
                        {
                            unit.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            unit.unitGroup = null;
                        }

                        unitGroup.unitSet.Clear();
                    }
                }
            }
        }
    }

    public void StopUnit(Unit unit)
    {
        if (unit.unitGroup != null)
        {
            unit.GetComponent<Rigidbody>().velocity = Vector3.zero;
            unit.unitGroup.RemoveUnit(unit);
            unit.unitGroup = null;
        }
    }
    
}