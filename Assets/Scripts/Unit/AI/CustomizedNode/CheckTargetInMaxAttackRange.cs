using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetInMaxAttackRange : Node
{
    float attackRange;
    Unit unit;
    readonly PlayerRef myOwner;
    int unitMask;

    public CheckTargetInMaxAttackRange(Unit unit) : base()
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
            //Debug.Log("distance to enemy: " + distance);
            return NodeState.SUCCESS;
        }
        //Debug.Log(distance + " Not within attack range " + attackRange);
        return NodeState.FAILURE;
    }

    }
