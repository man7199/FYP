using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UIElements;

public class Mission : NetworkBehaviour
{
    public static Mission Current { get; private set; }

    public Transform[] spawnpoints;

    public Transform CellSpawnPoint;
    public Transform RobotSpawnPoint;

    [Networked] public PlayerRef cellPlayerRef { get; set; }
    [Networked] public PlayerRef robotPlayerRef { get; set; }
    public PlayerRef localPlayer;

    public Building cellBase;
    public Building robotBase;
    public Building resourcePoint;

    public MissionDefinition definition;
    [SerializeField] List<GameObject> gameObjectList = new List<GameObject>();

    private void Awake()
    {
        Current = this;

        GameManager.SetMission(this);
    }

    public override void Spawned()
    {
        base.Spawned();
        localPlayer = Runner.LocalPlayer;
    }

    private void OnDestroy()
    {
        GameManager.SetMission(null);
    }

    public void SpawnPlayer()
    {
        BuildingController instance = BuildingController.Instance;

        IEnumerator WaitForFunction()
        {
            yield return new WaitForSeconds(1);
            int num = 0;
            foreach (PlayerRef player in Runner.ActivePlayers)
            {
                Debug.Log(player + "123123  " + num);
                if (num == 0)
                {
                    cellPlayerRef = player;
                    BuildingController.Instance.CalculateTransform(CellSpawnPoint.position,
                        out Vector3 initializePosition, cellBase);
                    instance.RpcPlacingBuilding(cellBase.prefabRef, CellSpawnPoint.position, player,
                        initializePosition);

                    foreach (GameObject positionStuff in gameObjectList)
                    {
                        Debug.Log("building");
                        instance.RpcPlacingBuilding(resourcePoint.prefabRef, positionStuff.transform.position, player, positionStuff.transform.position);
                        Debug.Log("building success");
                    }

                }
                else
                {
                    robotPlayerRef = player;
                    BuildingController.Instance.CalculateTransform(RobotSpawnPoint.position,
                        out Vector3 initializePosition, robotBase);

                    instance.RpcPlacingBuilding(robotBase.prefabRef, RobotSpawnPoint.position, player,
                        initializePosition);
                }

                num++;
            }
        }

        StartCoroutine(WaitForFunction());
    }
}