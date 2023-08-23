using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopUnit : Node
{
    private ShooterRobot unit;
    public StopUnit(ShooterRobot unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("stopppp");
        UnitController.Instance.StopUnit(unit);
        return NodeState.SUCCESS;
    }

}
