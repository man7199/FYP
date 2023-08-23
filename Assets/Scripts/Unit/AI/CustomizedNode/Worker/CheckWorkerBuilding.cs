using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWorkerBuilding : Node
{
    private RedBloodCell worker;

    public CheckWorkerBuilding(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        if (worker.CheckState() == RedBloodCell.WorkType.Building)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
