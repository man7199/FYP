
    using UnityEngine;
    [CreateAssetMenu(menuName = "Buffs/HealBuff")]
    public class ScriptableHealBuff:ScriptableBuff
    {
        public int healAmountPerSecond;
        public override Buff InitializeBuff(GameObject obj)
        {
            return new HealBuff(this, obj);
        }
    }
