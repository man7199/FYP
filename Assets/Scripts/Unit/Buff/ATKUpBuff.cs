using Fusion;
using UnityEngine;

public class ATKUpBuff : Buff
{
    
    public ATKUpBuff(ScriptableBuff buffData, GameObject obj) : base(buffData, obj)
    {
    }
    

    protected override void ApplyEffect()
    {
        ScriptableATKUpBuff scriptableAtkUpBuff = (ScriptableATKUpBuff) BuffData;
        buffableEntity.unit.ATK += scriptableAtkUpBuff.ATKUpAmount;
    }

    public override void End()
    {
        ScriptableATKUpBuff scriptableAtkUpBuff = (ScriptableATKUpBuff) BuffData;
        buffableEntity.unit.ATK -= scriptableAtkUpBuff.ATKUpAmount * effectStacks;
    }

    
}