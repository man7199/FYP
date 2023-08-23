using System.Collections.Generic;
using System.Linq;
using UnityEngine;


using Fusion;

public class CheckEnemyInAttackRange : Node
{
    readonly Unit unit;
    float attackRange;
    readonly PlayerRef myOwner;

    Vector3 currentPosition;

    public CheckEnemyInAttackRange(Unit unit) : base()
    {
        this.unit=unit;
        attackRange = unit.attackRange;
        myOwner =unit.Owner;
    }

    public override NodeState Evaluate()
    {
        Transform currentTarget = unit.target;
        // if (currentTarget == null)
        // {
        //     _state = NodeState.FAILURE;
        //     return _state;
        // }
        // Transform target = (Transform)currentTarget;

        // (in case the target object is gone - for example it died
        // and we haven't cleared it from the data yet)
        if (!currentTarget)
        {
            _state = NodeState.FAILURE;
            return _state;
        }


        float d = Vector3.Distance(unit.transform.position, currentTarget.position);
        bool isInRange = (d <= attackRange); 
        _state = isInRange ? NodeState.SUCCESS : NodeState.FAILURE;
        return _state;

    }
}