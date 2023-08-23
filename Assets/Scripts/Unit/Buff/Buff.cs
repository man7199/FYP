using UnityEngine;

public abstract class Buff
{
    protected float duration;
    // 層數
    protected int effectStacks;
    public ScriptableBuff BuffData { get; }
    protected readonly BuffableEntity buffableEntity;

    public bool IsFinished;
    public string description;
    public Sprite image;
    public Buff(ScriptableBuff buffData, GameObject obj)
    {
        BuffData = buffData;
        buffableEntity = obj.GetComponent<BuffableEntity>();
    }

    public virtual void Tick(float deltaTime)
    {
        duration -= deltaTime;
        if (duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    /**
     * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
     */
    public void Activate()
    {
        
        if (duration <= 0 || BuffData.IsEffectStacked)
        {
            ApplyEffect();
            duration = BuffData.Duration;
            Debug.Log("Duration");
            Debug.Log(duration);
            effectStacks++;
            description = BuffData.description;
            image = BuffData.image;
        }
        else if (BuffData.IsDurationStacked)
        {
            duration += BuffData.Duration;
        }
    }
    protected abstract void ApplyEffect();
    public abstract void End();

    public float getDuration() {
        return duration;
    }
}