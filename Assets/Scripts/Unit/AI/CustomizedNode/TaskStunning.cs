using UnityEngine;
using System.Collections.Generic;


public class TaskStunning : Node
{
    private Unit _unit;
    private float _timer = 0;

    public TaskStunning(Unit unit)
    {
        _unit = unit;
    }

    public override NodeState Evaluate()
    {
        // The action to do when stunning. Nothing to do yet.
        
        _state = NodeState.SUCCESS;
        return _state;
    }
}