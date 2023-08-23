
    using UnityEngine;
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyWaveScriptableObject", order = 3)]
    public class EnemyWaveScriptableObject:ScriptableObject
    {
        public EnemyGroupScriptableObject[] enemyGroups;
        public float waveInterval;
    }
