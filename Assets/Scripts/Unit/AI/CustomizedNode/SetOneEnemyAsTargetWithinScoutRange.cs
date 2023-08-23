using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class SetOneEnemyAsTargetWithinScoutRange : Node
{
    float range;
    Unit unit;
    readonly PlayerRef myOwner;
    int unitMask;
    public SetOneEnemyAsTargetWithinScoutRange( Unit unit ) : base()
    {
        this.range = unit.scoutRange;
        this.unit = unit;
        unitMask = LayerMask.GetMask("Unit");
    }

    public override NodeState Evaluate()
    {
        var affectedObjects = Physics.OverlapSphere(unit.transform.position, range, unitMask);
        foreach (var col in affectedObjects)
        {
            Unit newTarget = col.GetComponent<Unit>();
            if (newTarget.teamType != unit.teamType)
            {
                Debug.Log("new target obtained! " + newTarget.name);
                //Debug.Log("new target of " + unit.ToString() + " is: " + unit.target.ToString());
                unit.target = newTarget.transform;
                return NodeState.SUCCESS;
            }
        }
        //Debug.Log("no new target obtained! " + unit.target.name);
        return NodeState.FAILURE;
    }



}
