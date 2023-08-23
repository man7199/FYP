

    using UnityEngine;
    [CreateAssetMenu(menuName = "Buffs/ATKUpBuff")]
    public class ScriptableATKUpBuff:ScriptableBuff
    {
        public int ATKUpAmount;
        public override Buff InitializeBuff(GameObject obj)
        {
            return new ATKUpBuff(this, obj);
        }
    }
