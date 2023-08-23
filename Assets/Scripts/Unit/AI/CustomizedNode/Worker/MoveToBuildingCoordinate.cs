using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBuildingCoordinate : Node
{
    RedBloodCell unit;

    public MoveToBuildingCoordinate(RedBloodCell unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(unit.transform.position, unit.GetbuildingCoordinate());
        if (distance > RedBloodCell.maxBuildDistance)
        {
            unit.MoveTowardsBuildingCoordinate();
        }

        return NodeState.SUCCESS;

    }

}
