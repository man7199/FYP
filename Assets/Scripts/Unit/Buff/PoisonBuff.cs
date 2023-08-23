using Fusion;
using UnityEngine;

public class PoisonBuff : Buff
{
    private float _healTimer;
    public PoisonBuff(ScriptableBuff buffData, GameObject obj) : base(buffData, obj)
    {
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        _healTimer += deltaTime;
        if (_healTimer >=1)
        {
            ScriptablePoisonBuff buffData = (ScriptablePoisonBuff) BuffData;
            buffableEntity.unit.TakeDamage(buffData.damagePerSecond*effectStacks,0,null);  
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