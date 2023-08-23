using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnsSuccess : Node
{
    public ReturnsSuccess()
    {

    }

    public override NodeState Evaluate()
    {
            return NodeState.SUCCESS;
    }
}
