using UnityEngine;

public class StunBuff : Buff
{
    public StunBuff(ScriptableBuff buffData, GameObject obj) : base(buffData, obj)
    {
    }

    protected override void ApplyEffect()
    {
        buffableEntity.IsStunning = true;
    }

    public override void End()
    {
        effectStacks = 0;
        buffableEntity.IsStunning = false;
        buffableEntity.unit.WakeUpFromStun();
    }
}