using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Fusion;

public class TaskScout : Node
{//task set enemy in field of view
    readonly Unit unit;
    float _fovRadius;

    Vector3 currentPosition;

    public TaskScout(Unit unit) : base()
    {
        this.unit = unit;
        _fovRadius = unit.scoutRange;
    }

    public override NodeState Evaluate()
    {
        _state = NodeState.FAILURE;
        
        currentPosition = unit.transform.position;
        if (unit.target != null)
        {
            float distance = Vector3.Distance(unit.target.position,currentPosition);
            if (distance > unit.scoutMaxRange)
            {
                unit.target = null;
            }
            else
            {
                return _state;
            }
        }
        IEnumerable<Collider> enemiesInRange =
            Physics.OverlapSphere(currentPosition, _fovRadius, Global.UNIT_MASK)
                .Where(delegate(Collider c)
                {
                    Unit targetUnit = c.GetComponent<Unit>();
                    if (targetUnit == null) return false;
                    return targetUnit.teamType != unit.teamType;
                });
        if (enemiesInRange.Any())
        {
            unit.target = enemiesInRange
                .OrderBy(x => (x.transform.position - currentPosition).sqrMagnitude)
                .First()
                .transform;
        }

        
        return _state;
    }
}