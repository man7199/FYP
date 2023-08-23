using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateResourcePoint : Node
{
    private RedBloodCell worker;

    public UpdateResourcePoint(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        bool success = worker.UpdateResourcePoint();
        if (success) 
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
