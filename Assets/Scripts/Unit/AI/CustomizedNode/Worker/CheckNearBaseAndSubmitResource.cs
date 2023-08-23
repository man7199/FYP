using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class CheckNearBaseAndSubmitResource : Node
{
    private RedBloodCell worker;
    private float distance = ResourceCollectionPoint.submitDistance;
    public CheckNearBaseAndSubmitResource(RedBloodCell worker)
    {
        this.worker = worker;
    }

    public override NodeState Evaluate()
    {
        if(worker.CheckWorkerDistanceToBase() <= distance)
        {
            worker.SendResourcesToBase();
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
