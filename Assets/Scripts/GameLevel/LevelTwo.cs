using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwo : GameLevel
{
    private static HashSet<VirusBase> Destoryed = new HashSet<VirusBase>();
    public VirusBase[] spawnpoints;
    public Transform[] destination;
    private float timer;
    private int index;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        bool? isServer = Runner?.IsServer;
        if (isServer.HasValue && isServer.Value)
        {
            Debug.Log(isServer.HasValue);
            Debug.Log(isServer.Value);
            StartCoroutine(SpawnEnemy());
        }
    }

    private void Update()
    {
        if (Destoryed.Count == 5)
        {
            WinCondition();
        }

        timer = Random.Range(3, 10);
        index = Random.Range(0, destination.Length - 1);
    }

    UnitGroup unitGroup = null;

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Debug.Log(timer);
            Debug.Log(index);

            // foreach (EnemyGroupScriptableObject enemyGroup in wave.enemyGroups)
            // {
            //     if (unitGroup != null)
            //     {
            //         HashSet<Unit> spawnEnemyGroup = SpawnEnemyGroup(enemyGroup);
            //         unitGroup.AddUnits(spawnEnemyGroup);
            //         foreach (Unit unit in spawnEnemyGroup)
            //         {
            //             unit.unitGroup = unitGroup;
            //         }
            //     }
            //     else
            //     {
            //         UnitController.Instance.MoveUnit(SpawnEnemyGroup(enemyGroup), destination.position, false,
            //             out UnitGroup normalUnitGroup, out UnitGroup passWallUnitGroup);
            //         unitGroup = normalUnitGroup;
            //     }
            //
            //     yield return new WaitForSeconds(enemyGroup.spawnTime);
            // }

            foreach (VirusBase spawnpoint in spawnpoints)
            {
                //Debug.Log(spawnpoint);
                if (spawnpoint != null)
                {
                    for (int i = 0; i < Random.Range(0, 2); i++)
                    {
                        Unit unit = spawnpoint.SpawnRandom();
                        if (unitGroup != null)
                        {
                            unitGroup.AddUnit(unit);
                            unit.unitGroup = unitGroup;
                        }
                        else
                        {
                            HashSet<Unit> units = new HashSet<Unit>() { unit };
                            UnitController.Instance.MoveUnit(units, destination[index].position, false,
                                out UnitGroup normalUnitGroup, out UnitGroup passWallUnitGroup);
                            unitGroup = normalUnitGroup;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(timer);
        }
    }

    public static void DestroyedSpawn(VirusBase virusBase)
    {
        Destoryed.Add(virusBase);
        Debug.Log(Destoryed);
    }

    private void WinCondition()
    {
        canvas.ShowVictory();
    }
}