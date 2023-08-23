

    using UnityEngine;
    [CreateAssetMenu(menuName = "Buffs/AOEUpBuff")]
    public class ScriptableAOEUpBuff:ScriptableBuff
    {
        public int AOEUpPercentage;
        public override Buff InitializeBuff(GameObject obj)
        {
            return new AOEUpBuff(this, obj);
        }
    }
