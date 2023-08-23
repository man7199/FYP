using Fusion;
using UnityEngine;

public class HealBuff : Buff
{
    private float _healTimer;
    public HealBuff(ScriptableBuff buffData, GameObject obj) : base(buffData, obj)
    {
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        _healTimer += deltaTime;
        if (_healTimer >=1)
        {
            ScriptableHealBuff buffData = (ScriptableHealBuff) BuffData;
            buffableEntity.unit.regenHP(buffData.healAmountPerSecond * effectStacks);  // EffectStacks 層數
            _healTimer = 0;
        }
    }

    protected override void ApplyEffect()
    {
        //nothing to do
    }

    public override void End()
    {
        // nothing to do
    }
}