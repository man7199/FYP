using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Buff", order = 0)]
    public abstract class ScriptableBuff : ScriptableObject
    {
        /**
     * Time duration of the buff in seconds.
     */
        public float Duration;

        /**
     * Duration is increased each time the buff is applied.
     */
        public bool IsDurationStacked;

        /**
     * Effect value is increased each time the buff is applied. Also, the duration will be refreshed.
     */
        public bool IsEffectStacked;

        
  
        /// <summary>
        /// Use for creating buff instance apply on obj
        /// </summary>
        public abstract Buff InitializeBuff(GameObject obj);

        public string description;
        public Sprite image;
}
