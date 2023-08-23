
using UnityEngine;
using UnityEngine.UI;

public class CheckHasTarget : Node
{
    private Unit unit;

    public CheckHasTarget(Unit unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        if (unit.target == null)
        {
            _state = NodeState.FAILURE;
            return _state;
        }
        _state = NodeState.SUCCESS;
        return _state;
    }
}