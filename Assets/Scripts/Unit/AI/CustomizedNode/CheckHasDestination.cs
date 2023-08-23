
using UnityEngine;
using UnityEngine.UI;

public class CheckHasDestination : Node
{
    private Unit unit;

    public CheckHasDestination(Unit unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        if (unit.unitGroup != null)
        {
            _state = NodeState.SUCCESS;
            return _state;
        }

        _state = NodeState.FAILURE;
        return _state;
    }
}