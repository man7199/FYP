
    using UnityEngine;
    [CreateAssetMenu(menuName = "Buffs/PoisonBuff")]
    public class ScriptablePoisonBuff:ScriptableBuff
    {
        public int damagePerSecond;
        public override Buff InitializeBuff(GameObject obj)
        {
            return new PoisonBuff(this, obj);
        }
    }
