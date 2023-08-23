using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorkerMoveToMineOrBase : Node
{
    RedBloodCell worker;

    public SetWorkerMoveToMineOrBase(RedBloodCell unit) : base()
    {
        this.worker = unit;
    }

    public override NodeState Evaluate()
    {
        worker.MoveToMineOrBase();
        return NodeState.SUCCESS;

    }

}
