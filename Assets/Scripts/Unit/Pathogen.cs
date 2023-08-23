using UnityEngine;

public abstract class Pathogen : Unit
{
    [SerializeField]private LevelOne.SpawnPoint spawnPoint=LevelOne.SpawnPoint.NULL;

    protected override void Death()
    {
        base.Death();
        if (spawnPoint != LevelOne.SpawnPoint.NULL)
        {
            LevelOne.DestroySpawnPoint(spawnPoint);
        }
    }
}