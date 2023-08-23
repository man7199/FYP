using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : Node
{
    RedBloodCell unit;

    public Build(RedBloodCell unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        bool success = unit.Build();
        if (success)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }

}
