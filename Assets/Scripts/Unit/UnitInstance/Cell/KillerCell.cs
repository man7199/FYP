using UnityEngine;

public class KillerCell : Cell, IMelee
{
    protected override void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Cell/nkcell");
    }
    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }
    
    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm Killer Cell, which is a type of cell. I'm a friendly unit which performs melee attacks to a close-by enemy.";
    public override string getInfo()
    {
        return UnitInfo;
    }

    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>()?.TakeDamage(ATK, Owner,this);
    }
}