using Fusion;
using UnityEngine;

public class SpeedUpBuff : Buff
{
    private float originSpeed;
    public SpeedUpBuff(ScriptableBuff buffData, GameObject obj) : base(buffData, obj)
    {
    }
    
    protected override void ApplyEffect()
    {
        ScriptablSpeedUpBuff buffData = (ScriptablSpeedUpBuff) BuffData;
        originSpeed = buffableEntity.unit.moveSpeed;
        buffableEntity.unit.moveSpeed *= buffData.SpeedUpAmount;
    }

    public override void End()
    {
        buffableEntity.unit.moveSpeed = originSpeed;
    }
}