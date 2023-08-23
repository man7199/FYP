using System.Collections.Generic;

using UnityEngine;


public class TaskChaseTarget : Node
{
    private Unit unit;
    private Transform myTransform;
    private Rigidbody myRigidbody;
    private float speed;
    public TaskChaseTarget(Unit unit)
    {
        this.unit = unit;
        myTransform = unit.GetComponent<Transform>();
        myRigidbody = unit.GetComponent<Rigidbody>();
        speed = unit.moveSpeed;
    }

    public override NodeState Evaluate()
    {
        Transform target = unit.target;
        if (target == null)
        {
            _state =NodeState.FAILURE;
            return _state;
        }
         
        Vector3 targetPosition = target.position;
        Vector3 myPosition = myTransform.position;
        Vector3 direction = new Vector3(targetPosition.x-myPosition.x,0,targetPosition.z-myPosition.z).normalized;
        myRigidbody.velocity = direction * speed;
        if (direction.magnitude != 0) unit.TurnTowardsCoordinate(targetPosition);
        //     unit.transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
        _state =NodeState.SUCCESS;
        return _state;
    }
}