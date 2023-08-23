using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetInAttackRange : Node
{
    float attackRange;
    Unit unit;
    readonly PlayerRef myOwner;
    int unitMask;

    public CheckTargetInAttackRange(Unit unit) : base()
    {
        this.attackRange = unit.attackRange;
        this.unit = unit;
        unitMask = LayerMask.GetMask("Unit");
    }

    public override NodeState Evaluate()
    {
        Transform currentTarget = unit.target;
        float distance = Vector3.Distance(unit.transform.position, currentTarget.position);
        if (distance <= attackRange)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }

    }
