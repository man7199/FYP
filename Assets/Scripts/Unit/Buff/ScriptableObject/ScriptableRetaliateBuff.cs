

    using UnityEngine;
    [CreateAssetMenu(menuName = "Buffs/RetaliateBuff")]
    public class ScriptableRetaliateBuff:ScriptableBuff
    {
        [Range(0,1)]
        public float retaliatePercentage;
        
        public override Buff InitializeBuff(GameObject obj)
        {
            return new RetaliateBuff(this, obj);
        }
    }
