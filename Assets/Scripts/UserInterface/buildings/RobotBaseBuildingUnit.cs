using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBaseBuildingUnit : BuildingUnit
{
    private bool isBuilding = false;
    public BuildingGhost bg;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        bg = GameObject.Find("BuildingGhost").GetComponent<BuildingGhost>();
    }

    public override void Skill1()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(9);
    }
    public override void Skill2()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(10);
    }
    public override void Skill3()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(11);
    }
    public override void Skill4()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(12);
    }
    public override void Skill5()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(13);
    }

    public void Skill7()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(14);
    }
    public void Skill8()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(15);
    }
    public void Skill9()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(16);
    }

}