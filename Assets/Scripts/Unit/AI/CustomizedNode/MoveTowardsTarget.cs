using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsTarget: Node
{
    private Unit unit;
    private Transform target;

    public MoveTowardsTarget(Unit unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        unit.GetComponent<Rigidbody>().velocity = unit.transform.forward * unit.moveSpeed;
        return NodeState.SUCCESS;

    }

}
