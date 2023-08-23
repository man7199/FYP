using Fusion;
using UnityEngine;


public class BuildingUnit : Unit
{
    private Building myBuilding;
    private GameUI canvas;

    protected override Node SetupBehaviorTree()
    {
        return new TaskIdle(this);
    }

    public override void Spawned()
    {
        base.Spawned();
        canvas = GameObject.Find("Utility").GetComponentInChildren<GameUI>();
        myBuilding = GetComponentInParent<Building>();
        Owner = myBuilding.Owner;
    }

    public override string getInfo()
    {
        return "Unit used for building";
    }

    public override bool TakeDamage(int damage, PlayerRef playerRef, Unit unit)
    {
        Debug.Log("Taking damage" + damage);
        myBuilding.currenthp -= damage;
        if (myBuilding.currenthp <= 0)
        {
            if (myBuilding.GetType() == typeof(MotherBase))
            {
                RPCShowDefeat();
            }

            BuildingController.Instance.DestroyBuilding(myBuilding);
        }

        return false;
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPCShowDefeat()
    {
        canvas.ShowDefeat();
    }
    /*
     * 
     * public virtual void Build(Vector3 position, Building building)
    {
        if (buildingUI.Skill1(resource.GetResource()))
        {
            uiprogress.addQueue(image[0].GetComponent<Image>());
        }
        else
        {
            
        }
    }

    }
     */
}