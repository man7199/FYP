using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class SetOneEnemyAsTargetWithinMinRange : Node
{
    float range;
    Unit unit;
    readonly PlayerRef myOwner;
    int unitMask;
    public SetOneEnemyAsTargetWithinMinRange( Unit unit ) : base()
    {
        this.range = unit.minAttackRange;
        this.unit = unit;
        unitMask = LayerMask.GetMask("Unit");
    }

    public override NodeState Evaluate()
    {
        var affectedObjects = Physics.OverlapSphere(unit.transform.position, range, unitMask);
        foreach (var col in affectedObjects)
        {
            Unit target = col.GetComponent<Unit>();
            if (target.teamType != unit.teamType)
            {
                unit.target = target.transform;
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }



}
