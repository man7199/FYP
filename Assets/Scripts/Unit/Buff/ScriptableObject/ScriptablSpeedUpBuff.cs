

    using UnityEngine;
    [CreateAssetMenu(menuName = "Buffs/SpeedUpBuff")]
    public class ScriptablSpeedUpBuff:ScriptableBuff
    {
        public float SpeedUpAmount;
        public override Buff InitializeBuff(GameObject obj)
        {
            return new SpeedUpBuff(this, obj);
        }
    }
