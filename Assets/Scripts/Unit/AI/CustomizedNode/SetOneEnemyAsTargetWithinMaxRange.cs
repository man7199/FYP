using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class SetOneEnemyAsTargetWithinMaxRange : Node
{//use this when the minRange is 0
    float range;
    Unit unit;
    int unitMask;
    public SetOneEnemyAsTargetWithinMaxRange( Unit unit ) : base()
    {
        this.range = unit.attackRange;
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
                Debug.Log("find new target: " + target.name);
                unit.target = target.transform;
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }



}
