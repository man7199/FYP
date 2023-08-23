using Fusion;
using UnityEngine;

public class SmallerBacteria : Pathogen, IMelee
{
    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    private void Awake()
    {
        base.Awake();
        icon = Resources.Load<Sprite>("Icons/Enemy/bacteria");
    }
    public void MeleeAttack(Transform target)
    {
        target?.GetComponent<Unit>().TakeDamage(ATK, Owner, this);
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm smaller bacteria";
    public override string getInfo()
    {
        return UnitInfo;
    }
}