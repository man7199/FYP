using UnityEngine;
using System.Collections.Generic;


public class TaskIdle : Node
{
    private Unit _unit;

    public TaskIdle(Unit unit)
    {
        _unit = unit;
    }

    public override NodeState Evaluate()
    {
        _state = NodeState.SUCCESS;
        return _state;
    }
}