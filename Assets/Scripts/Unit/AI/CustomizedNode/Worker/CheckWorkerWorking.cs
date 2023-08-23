using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWorkerWorking : Node
{
    private RedBloodCell worker;

    public CheckWorkerWorking(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        if (worker.CheckState()!= RedBloodCell.WorkType.Idle)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
