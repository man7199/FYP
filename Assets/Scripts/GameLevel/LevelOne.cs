using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class LevelOne : GameLevel
{
    public enum SpawnPoint
    {
        LEFT,
        RIGHT,
        LEFT_BOSS,
        RIGHT_BOSS,
        MIDDLE_BOSS,
        NULL
    }

    private static HashSet<SpawnPoint> distroyedPoint = new HashSet<SpawnPoint>();
    public EnemyWaveScriptableObject[] waves;
    public int nextWaveIndex = 0;
    private float timer;


    public Transform destination;
    public Transform leftPoint, rightPoint, leftBossPoint, rightBossPoint, middleBossPoint;
    private Transform[] spawnPoints;

    private void Update()
    {
        bool? isServer = Runner?.IsServer;
        if (isServer.HasValue && isServer.Value)
        {
            timer += Time.deltaTime;
            if (nextWaveIndex < waves.Length && timer > waves[nextWaveIndex].waveInterval)
            {
                StartCoroutine(SpawnEnemyWave(waves[nextWaveIndex]));
                nextWaveIndex++;
            }

            if (nextWaveIndex == waves.Length)
            {
                RPCShowVictory();
            }
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPCShowVictory()
    {
        canvas.ShowVictory();

    }

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            spawnPoints = new Transform[5];
            spawnPoints[0] = leftPoint;
            spawnPoints[1] = rightPoint;
            spawnPoints[2] = leftBossPoint;
            spawnPoints[3] = rightBossPoint;
            spawnPoints[4] = middleBossPoint;
        }
    }

    public static void DestroySpawnPoint(SpawnPoint spawnPoint)
    {
        distroyedPoint.Add(spawnPoint);
    }


    IEnumerator SpawnEnemyWave(EnemyWaveScriptableObject wave)
    {
        UnitGroup unitGroup = null;
        foreach (EnemyGroupScriptableObject enemyGroup in wave.enemyGroups)
        {
            if (unitGroup != null)
            {
                HashSet<Unit> spawnEnemyGroup = SpawnEnemyGroup(enemyGroup);
                unitGroup.AddUnits(spawnEnemyGroup);
                foreach (Unit unit in spawnEnemyGroup)
                {
                    unit.unitGroup = unitGroup;
                }
            }
            else
            {
                UnitController.Instance.MoveUnit(SpawnEnemyGroup(enemyGroup), destination.position, false,
                    out UnitGroup normalUnitGroup, out UnitGroup passWallUnitGroup);
                unitGroup = passWallUnitGroup;
            }

            yield return new WaitForSeconds(enemyGroup.spawnTime);
        }
    }

    private HashSet<Unit> SpawnEnemyGroup(EnemyGroupScriptableObject enemyGroup)
    {
        HashSet<Unit> units = new HashSet<Unit>();
        foreach (EnemyScriptableObject enemy in enemyGroup.enemy)
        {
            foreach (SpawnPoint point in enemyGroup.spawnPoints)
            {
                if (distroyedPoint.Contains(point))
                {
                    continue;
                }

                for (int i = 0; i < enemy.numberOfPrefabsToCreate; i++)
                {
                    Unit newUnit = Runner.Spawn(enemy.prefab,
                        spawnPoints[(int)point].position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)),
                        Quaternion.identity).GetComponent<Unit>();
                    units.Add(newUnit);
                }
            }
        }

        return units;
    }
}