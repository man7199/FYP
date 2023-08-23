

    using UnityEngine;
    [CreateAssetMenu(menuName = "Buffs/StunBuff")]
    public class ScriptableStunBuff:ScriptableBuff
    {
        public override Buff InitializeBuff(GameObject obj)
        {
            return new StunBuff(this, obj);
        }
    }
