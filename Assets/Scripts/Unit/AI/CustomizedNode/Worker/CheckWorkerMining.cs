using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWorkerMining : Node
{
    private RedBloodCell worker;

    public CheckWorkerMining(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        if (worker.CheckState() == RedBloodCell.WorkType.Mining)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
