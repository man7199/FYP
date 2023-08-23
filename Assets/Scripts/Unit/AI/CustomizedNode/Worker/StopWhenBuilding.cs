using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWhenBuilding : Node
{
    float minDistance;
    RedBloodCell unit;

    public StopWhenBuilding(RedBloodCell unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        if (unit.CheckState() == RedBloodCell.WorkType.Building)
        {
            if (unit.GetHasBuildTask())
            {
                float distance = Vector3.Distance(unit.transform.position, unit.GetbuildingCoordinate());
                if (distance <= RedBloodCell.maxBuildDistance - 3f)
                {
                    UnitController.Instance.StopUnit(unit);
                }
            }
        }
        return NodeState.SUCCESS;
    }

}
