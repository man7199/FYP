using UnityEngine;
using System.Collections.Generic;


public class FixedTimer : Node
{
    private float _delay;
    private float _time;

    public delegate void TickEnded();

    public event TickEnded onTickEnded;

    public FixedTimer(float delay, TickEnded onTickEnded = null)
    {
        _delay = delay;
        _time = 0;
        this.onTickEnded = onTickEnded;
    }

    public FixedTimer(float delay, Node children, TickEnded onTickEnded = null) : this(delay, onTickEnded)
    {
        SetChildren(new List<Node> { children });
    }

    public override NodeState Evaluate()
    {
        if (!HasChildren) return NodeState.FAILURE;
        if (_time <= 0)
        {
            _time = _delay;
            _state = children[0].Evaluate();
            if (onTickEnded != null)
                onTickEnded();
            _state = NodeState.SUCCESS;
        }
        else
        {
            _time -= Time.deltaTime;
            _state = NodeState.SUCCESS;
        }

        return _state;
    }
}