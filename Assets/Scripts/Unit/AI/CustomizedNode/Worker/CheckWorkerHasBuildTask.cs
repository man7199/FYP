using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWorkerHasBuildTask : Node
{
    private RedBloodCell worker;

    public CheckWorkerHasBuildTask(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        if (worker.GetHasBuildTask())
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
