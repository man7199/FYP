using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckEnemyInRecessionRange : Node
{
    float recessionRange;
    Unit unit;
    readonly PlayerRef myOwner;

    public CheckEnemyInRecessionRange(Unit unit) : base()
    {
        recessionRange = unit.minAttackRange;
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        IEnumerable<Collider> enemiesInRange =
            Physics.OverlapSphere(unit.transform.position, unit.minAttackRange, Global.UNIT_MASK)
                .Where(delegate(Collider c)
                {
                    Unit targetUnit = c.GetComponent<Unit>();
                    if (targetUnit == null) return false;
                    return targetUnit.teamType != unit.teamType;
                });
        if (enemiesInRange.Any())
        {
            unit.target = enemiesInRange
                .OrderBy(x => (x.transform.position - unit.transform.position).sqrMagnitude)
                .First()
                .transform;
            return NodeState.SUCCESS;
        }


        return NodeState.FAILURE;
    }
}