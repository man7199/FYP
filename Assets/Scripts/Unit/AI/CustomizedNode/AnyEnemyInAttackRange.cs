using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unit;
using static UnityEngine.UI.GridLayoutGroup;

public class AnyEnemyInAttackRange : Node
{
    readonly Unit unit;
    float attackRange;
    readonly PlayerRef myOwner;

    int unitMask = LayerMask.GetMask("Unit");


    public AnyEnemyInAttackRange(Unit unit) : base()
    {
        this.unit = unit;
        attackRange = unit.attackRange;
        myOwner = unit.Owner;

    }

    public override NodeState Evaluate()
    {
        var affectedObjects = Physics.OverlapSphere(unit.transform.position, attackRange, unitMask );
        foreach (Collider col in affectedObjects)
        {
            Unit target = col.GetComponent<Unit>();
            if (target != null && unit.teamType != target.teamType)
            {
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;

    }
}
