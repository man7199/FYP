using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWhenTargetInAttackRange : Node
{
    float attackRange;
    Unit unit;
    readonly PlayerRef myOwner;
    int unitMask;

    public StopWhenTargetInAttackRange(Unit unit) : base()
    {
        this.attackRange = unit.attackRange;
        this.unit = unit;
        unitMask = LayerMask.GetMask("Unit");
    }

    public override NodeState Evaluate()
    {
        if (unit.target != null)
        {
            Transform currentTarget = unit.target;
            float distance = Vector3.Distance(unit.transform.position, currentTarget.position);
            if (distance <= attackRange)
            {
                UnitController.Instance.StopUnit(unit);
            }
        }
        return NodeState.SUCCESS;
    }

}
