using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOccupyingResourcePoint : Node
{
    private RedBloodCell worker;

    public CheckOccupyingResourcePoint(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        if (worker.OccupyingResourcePoint())
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
