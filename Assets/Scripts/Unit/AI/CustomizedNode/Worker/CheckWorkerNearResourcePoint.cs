using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWorkerNearResourcePoint : Node
{
    private RedBloodCell worker;
    private float distance;
    private ResourcePoint resourcePoint;
    public CheckWorkerNearResourcePoint(RedBloodCell worker)
    {
        this.worker = worker;
        distance= ResourcePoint.refillDistance;
        ResourcePoint resourcePoint = worker.GetOccupyingResourcePoint();
    }

    public override NodeState Evaluate()
    {
        if (resourcePoint != null)
        {
            if (worker.CheckWorkerDistanceToResourcePoint() <= distance)
            {
                Debug.Log("near the point");
                return NodeState.SUCCESS;
            }
        }
        Debug.Log("not near the point");
        return NodeState.FAILURE;
    }
}
