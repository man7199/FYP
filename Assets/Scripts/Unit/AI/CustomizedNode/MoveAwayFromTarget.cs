using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAwayFromTarget<T> : Node where T : ShooterRobot
{
    private T unit;
    private Transform target;

    public MoveAwayFromTarget(T unit) : base()
    {
        this.unit = unit;
        this.target = unit.target;
    }

    public override NodeState Evaluate()
    {
        unit.MoveBackward();
        return NodeState.SUCCESS;
    }

}
