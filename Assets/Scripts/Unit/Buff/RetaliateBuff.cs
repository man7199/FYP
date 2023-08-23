using UnityEngine;

public class RetaliateBuff : Buff
{
    public RetaliateBuff(ScriptableRetaliateBuff buffData, GameObject obj) : base(buffData, obj)
    {
    }

    protected override void ApplyEffect()
    {
        buffableEntity.IsRetaliateActive = true;
    }

    public override void End()
    {
        effectStacks = 0;
        buffableEntity.IsRetaliateActive = false;
        buffableEntity.unit.WakeUpFromStun();
    }
}