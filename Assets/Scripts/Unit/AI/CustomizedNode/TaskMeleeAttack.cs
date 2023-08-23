using UnityEngine;

public class TaskMeleeAttack<T>  : Node where T:Unit,IMelee
{

    private T unit;
    private Transform target;
    private ProjectileStatScriptableObject projectileStat;

    public TaskMeleeAttack(T unit) : base()
    {
        this.unit = unit;
        this.projectileStat = projectileStat;
    }

    public override NodeState Evaluate()
    {
        unit.MeleeAttack(unit.target);
        _state = NodeState.SUCCESS;
        return _state;
    }
}