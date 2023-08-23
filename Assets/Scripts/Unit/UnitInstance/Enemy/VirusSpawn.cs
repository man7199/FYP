using Fusion;
using UnityEngine;


public class VirusSpawn : Unit
{
    private VirusBase myBuilding;
    protected override Node SetupBehaviorTree()
    {
        return new TaskIdle(this);
    }

    public override void Spawned()
    {
        base.Spawned();
        myBuilding = GetComponentInParent<VirusBase>();
        //Owner = myBuilding.Owner;
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
            BuildingController.Instance.DestroyBuilding(myBuilding);
            LevelTwo.DestroyedSpawn(myBuilding);
        }
        return false;
    } 
        
}
