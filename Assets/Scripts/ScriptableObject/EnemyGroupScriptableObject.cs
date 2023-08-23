using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyGroupScriptableObject", order = 2)]
public class EnemyGroupScriptableObject : ScriptableObject
{
    public EnemyScriptableObject[] enemy;
    public float spawnTime;
    public LevelOne.SpawnPoint[] spawnPoints;
}