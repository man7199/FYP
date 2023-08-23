using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMediumRangeAttackTarget<T> : Node where T: Unit, IMediumRange
{
    private T unit;
    public TaskMediumRangeAttackTarget(T unit): base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        unit.RangeAttackTarget(unit.target);
        return NodeState.SUCCESS;
    }
}
