using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


public class SetOneEnemyAsTargetWithinRange : Node
{
    float range;
    float minAttackRange;
    Unit unit;
    readonly PlayerRef myOwner;
    int unitMask;
    public SetOneEnemyAsTargetWithinRange( Unit unit ) : base()
    {
        this.range = unit.attackRange;
        this.minAttackRange = unit.minAttackRange;
        this.unit = unit;
        unitMask = LayerMask.GetMask("Unit");
        myOwner = unit.Owner;
    }

    public override NodeState Evaluate()
    {
        
        var affectedObjects = Physics.OverlapSphere(unit.transform.position, range, unitMask);

        foreach (var col in affectedObjects)
        {
            Unit target = col.GetComponent<Unit>();

            if (target.teamType != unit.teamType)
            {
                if (Vector3.Distance(target.transform.position, unit.transform.position) >= minAttackRange)
                {
                    unit.target = target.transform;
                    return NodeState.SUCCESS;
                }
            }
        }
        return NodeState.FAILURE;
    }



}
