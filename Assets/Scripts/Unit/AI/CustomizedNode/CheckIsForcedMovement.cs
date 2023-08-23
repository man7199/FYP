
using UnityEngine;
using UnityEngine.UI;

public class CheckIsForcedMovement : Node
{
    private Unit unit;

    public CheckIsForcedMovement(Unit unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        if (unit.isForcedMovement)
        {
            _state = NodeState.SUCCESS;
            return _state;
        }

        _state = NodeState.FAILURE;
        return _state;
    }
}