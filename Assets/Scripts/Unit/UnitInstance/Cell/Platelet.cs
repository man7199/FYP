using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Platelet : Cell
{
    [SerializeField] public Building ResourceCollectionPoint;
    [SerializeField] public Building BoneMarrow;
    [SerializeField] public Building Thymus;
    [SerializeField] public Building cellMembrane;
    [SerializeField] public Building cellWall;
    private BuildingGhost bg;
    private GameLevel gameLevel;

    int carryingResource = 0;


    protected override Node SetupBehaviorTree()
    {
        return Subtree.BasicSubtree(this);
    }


    protected override void Awake()
    {
        base.Awake();
        gameLevel = GameLevel.Instance;
        bg = GameObject.Find("Game Manager").GetComponentInChildren<BuildingGhost>();

        description[0] = "Resource collection point";
        description[1] = "Bone Marrow";
        //description[2] = "Thymus";
        description[3] = "Cell membrane";
        //description[4] = "Cell wall";
        icon = Resources.Load<Sprite>("Icons/Cell/platelet");
        sprite[0] = Resources.Load<Sprite>("Arts/UI/Building/resourcecollection");
        sprite[1] = Resources.Load<Sprite>("Arts/UI/Building/bonemarrow");
        sprite[3] = Resources.Load<Sprite>("Arts/UI/Building/cellmembrane");
        Unlock();
    }

    public override string getInfo()
    {
        return "Hello! I'm Platelet, which is a type of cell. I'm a friendly unit who can sacrifice to build a Resource Collection Point, a Bone Marrow, a Thymus, a Cell Membrane or a Cell Wall.";
    }

    public override void Skill1()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(ResourceCollectionPoint);
    }

    public override void Skill2()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(BoneMarrow);
    }

    public override void Skill3()
    {
        if (gameLevel.isThymusUnlock)
        {
            UI.setClicked(false);
            bg.SetBuildingGhost(Thymus);
        }
    }

    public override void Skill4()
    {
        UI.setClicked(false);
        bg.SetBuildingGhost(cellMembrane);
    }

    public override void Skill5()
    {
        if (gameLevel.isCellWallUnlock)
        {
            UI.setClicked(false);
            bg.SetBuildingGhost(cellWall);
        }
    }
    public void Unlock()
    {
        if (!gameLevel.isThymusUnlock)
        {
            description[2] = "Unlock in Mother Base to build Thymus";
            sprite[2] = Resources.Load<Sprite>("Arts/UI/Building/cannotthymus");

        }
        else
        {
            description[2] = "Thymus";
            sprite[2] = Resources.Load<Sprite>("Arts/UI/Building/thymus");

        }
        if (!gameLevel.isCellWallUnlock)
        {
            description[4] = "Unlock in Mother Base to build Cell Wall";
            sprite[4] = Resources.Load<Sprite>("Arts/UI/Building/cannotcellWall");

        }
        else
        {
            description[4] = "CellWall";
            sprite[4] = Resources.Load<Sprite>("Arts/UI/Building/cellWall");

        }
    }
}