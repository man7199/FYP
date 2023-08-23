using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class UnitSelection : MonoBehaviour
{
    public static UnitSelection Instance;

    public HashSet<Unit>[] unitTeams = new HashSet<Unit>[6];
    public HashSet<Unit> selectedUnits;
    public UnitUI UI;
    private int _unitMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There should be only one UnitController.");
            return;
        }

        selectedUnits = new HashSet<Unit>();
        Instance = this;

        for (int i = 0; i < unitTeams.Length; i++)
        {
            unitTeams[i] = new HashSet<Unit>();
        }
    }

    private void Start()
    {
        UI = GameObject.Find("UnitUI").GetComponent<UnitUI>();
        _unitMask = Global.UNIT_MASK;
    }

    public void SelectUnitsInTeam(int teamNum)
    {
        DeselectAll();
        foreach (Unit unit in unitTeams[teamNum])
        {
            SelectUnit(unit);
        }
    }

    public void AssignUnitsToTeam(int teamNum)
    {
        unitTeams[teamNum].Clear();
        foreach (Unit unit in selectedUnits)
        {
            unitTeams[teamNum].Add(unit);
            if (unit.unitTeamNum != -1 && unit.unitTeamNum != teamNum)
            {
                unitTeams[unit.unitTeamNum].Remove(unit);
            }

            unit.unitTeamNum = teamNum;
        }
    }

    public Unit GetUnitByWorldPosition(Vector3 position)
    {
        var ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out var hit, 50000.0f, _unitMask))
        {
            return hit.transform.GetComponent<Unit>();
        }

        return null;
    }

    public void MouseClickUnitSelection(Vector3 position, bool holdingShift = false)
    {
        Unit targetUnit = GetUnitByWorldPosition(position);
        if (targetUnit == null || targetUnit.GetType() == typeof(BuildingUnit))
        {
            UI.changeUI();
            return;
        }

        if (!holdingShift) DeselectAll();

        SelectUnit(targetUnit);
    }

    public void MouseDrag(Vector3 startPoint, Vector3 endPoint, bool holdingShift = false)
    {
        if (!holdingShift) DeselectAll();

        var corners = GetBoundingBox(startPoint, Input.mousePosition);
        var vertices = new Vector3[4];
        for (var i = 0; i < 4; i++)
        {
            vertices[i] = UtilsClass.GetMouseWorldPosition(corners[i]);
            if (vertices[i] == Vector3.zero) return;
        }

        var mesh = GenerateSelectionMesh(vertices);
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;


        Destroy(meshCollider, 1f);
    }

    private void SelectUnit(Unit unit)
    {
        //!!!
        ////Check if the player own the unit
        if (unit.Owner != Player.Instance.MyPlayerRef())
        {
            UI.changeUI(unit);
            return;
        }

        //if (unit && !selectedUnits.Contains(unit))
        //{

        unit.SetSelection(true);
        selectedUnits.Add(unit);
        UI.changeUI(unit);
        //}
    }

    public void DeselectUnit(Unit unit)
    {
        selectedUnits.Remove(unit);
    }


    private void DeselectAll()
    {
        foreach (var unit in selectedUnits) unit.SetSelection(false);
        selectedUnits.Clear();
    }

    private Mesh GenerateSelectionMesh(Vector3[] corners)
    {
        var boxHeight = 50.0f;
        var vertices = new Vector3[8];
        int[] triangles =
        {
            0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 4, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7
        };
        for (var i = 0; i < 4; i++) vertices[i] = corners[i];

        for (var i = 4; i < 8; i++) vertices[i] = corners[i - 4] + Vector3.up * boxHeight;

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    private Vector2[] GetBoundingBox(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 p1, p2, p3, p4;
        if (startPoint.x < endPoint.x)
        {
            if (startPoint.y < endPoint.y)
            {
                p1 = new Vector2(startPoint.x, endPoint.y);
                p2 = endPoint;
                p3 = startPoint;
                p4 = new Vector2(endPoint.x, startPoint.y);
            }
            else
            {
                p1 = startPoint;
                p2 = new Vector2(endPoint.x, startPoint.y);
                p3 = new Vector2(startPoint.x, endPoint.y);
                p4 = endPoint;
            }
        }
        else
        {
            if (startPoint.y < endPoint.y)
            {
                p1 = endPoint;
                p2 = new Vector2(startPoint.x, endPoint.y);
                p3 = new Vector2(endPoint.x, startPoint.y);
                p4 = startPoint;
            }
            else
            {
                p1 = new Vector2(endPoint.x, startPoint.y);
                p2 = startPoint;
                p3 = endPoint;
                p4 = new Vector2(startPoint.x, endPoint.y);
            }
        }

        Vector2[] corners = { p1, p2, p3, p4 };
        return corners;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            Unit unit = other.GetComponent<Unit>();
            if (unit.GetType() == typeof(BuildingUnit))
            {
                return;
            }

            SelectUnit(unit);
        }
    }
}