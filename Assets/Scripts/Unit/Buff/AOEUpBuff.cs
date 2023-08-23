using Fusion;
using UnityEngine;

public class AOEUpBuff : Buff
{
    private float increasedRange;

    public AOEUpBuff(ScriptableBuff buffData, GameObject obj) : base(buffData, obj)
    {
    }
    

    protected override void ApplyEffect()
    {
        ScriptableAOEUpBuff scriptableAtkUpBuff = (ScriptableAOEUpBuff) BuffData;
        increasedRange = buffableEntity.unit.explodeRange * scriptableAtkUpBuff.AOEUpPercentage;
        buffableEntity.unit.explodeRange += increasedRange;
    }

    public override void End()
    {
        buffableEntity.unit.explodeRange -= increasedRange;
    }

    
}