using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitResources : Node
{
    private RedBloodCell worker;
    public SubmitResources(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        worker.SendResourcesToBase();
        return NodeState.SUCCESS;
    }
}
