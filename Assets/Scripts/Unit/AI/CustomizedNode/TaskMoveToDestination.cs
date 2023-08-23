using UnityEngine;


public class TaskMoveToDestination : Node
{
    Unit unit;

    public TaskMoveToDestination(Unit unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        PathGrid pathGrid = unit.unitGroup.currentGrid;
        unit.Move(pathGrid);
        _state = NodeState.RUNNING;
        return _state;
    }
}