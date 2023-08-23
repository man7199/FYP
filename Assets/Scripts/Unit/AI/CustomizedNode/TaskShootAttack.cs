using UnityEngine;

public class TaskShootAttack<T>  : Node where T:Unit,IShooter
{

    private T unit;
    private Transform target;

    public TaskShootAttack(T unit) : base()
    {
        this.unit = unit;
    }

    public override NodeState Evaluate()
    {
        unit.ShootProjectile(unit.target);
        _state = NodeState.SUCCESS;
        return _state;
    }
}