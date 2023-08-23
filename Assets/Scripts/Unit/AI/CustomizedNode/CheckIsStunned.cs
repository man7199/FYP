using Fusion;
using UnityEngine;

public class CheckIsStunned:Node
{
    private BuffableEntity _entity;
    public CheckIsStunned(Unit unit)
    {
        _entity = unit.GetComponent<BuffableEntity>();
    }
    public override NodeState Evaluate()
    {
        if (_entity.IsStunning)
        {
            _state = NodeState.SUCCESS;
        }
        else
        {
            _state = NodeState.FAILURE;
        }

        return _state;
    }
}