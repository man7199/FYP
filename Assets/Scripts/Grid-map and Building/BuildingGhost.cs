using System;
using Fusion;
using UnityEngine;
using Utils;

public class BuildingGhost : MonoBehaviour
{
    private BuildingController buildingController;
    private Building currentBuilding;

    private GameObject visual;
    private bool isBuildingGhost = false;
    // Start is called before the first frame update
    private void Start()
    {

        buildingController = BuildingController.Instance;

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (isBuildingGhost)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        // can smoothen the movement
        buildingController.CalculateTransform(UtilsClass.GetMouseWorldPosition(), out var position,currentBuilding);
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, BuildingController.GetDirectionAngle(buildingController.GetDirection()), 0);
    }

    private void QOnTileChange(object sender, EventArgs args)
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        var building = buildingController.GetCurrentBuilding();
        if (building != null)
        {
            visual = Instantiate(building.gameObject, Vector3.zero, Quaternion.identity);
            visual.transform.SetParent(transform, false);
        }
    }
    public bool GetIsBuildingGhost() {
        return isBuildingGhost;
    }
    public void StopBuildingGhost() {
        Destroy(visual.gameObject);
        isBuildingGhost = false;
    }
    public void SetBuildingGhost() {
        isBuildingGhost = true;
        visual = Instantiate(buildingController.GetCurrentBuilding().gameObject, Vector3.zero, Quaternion.identity);
        visual.layer = LayerMask.GetMask("Default");
        visual.transform.SetParent(transform, false);
    }

    public Building GetCurrentBuilding()
    {
        return currentBuilding;
    }
    public void SetBuildingGhost(Building building)
    {
        if (isBuildingGhost)
            StopBuildingGhost();
        currentBuilding = building;
        isBuildingGhost = true;
        visual = Instantiate(building.gameObject, Vector3.zero, Quaternion.identity);
        
        int layer = LayerMask.GetMask("Default");
        var children = visual.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
        visual.transform.SetParent(transform, false);
    }
    public void SetBuildingGhost(int x)
    {
        if(isBuildingGhost)
        StopBuildingGhost();
        isBuildingGhost = true;
        //buildingController.OnBuildingChange += QOnTileChange;
        buildingController.SwitchBuilding(x);
        visual = Instantiate(buildingController.GetCurrentBuilding().gameObject, Vector3.zero, Quaternion.identity);
        visual.transform.SetParent(transform, false);
       
    }
}