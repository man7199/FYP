using UnityEngine;


public class NKCell : Cell, IMelee
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

    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>()?.TakeDamage(ATK, Owner,this);
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm someone who performs melee attack";
    public override string getInfo()
    {
        return UnitInfo;
    }
}