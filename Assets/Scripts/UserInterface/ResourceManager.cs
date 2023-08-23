using Fusion;
using TMPro;
using UnityEngine;

public class ResourceManager : NetworkBehaviour
{
    public static ResourceManager Instance;
    [Networked] private int cellResources { get; set; }
    [Networked] private int robotResources { get; set; }

    public TMP_Text text1;
    private Mission mission;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There should be only one BuildingController.");
            return;
        }

        Instance = this;
    }

    public override void Spawned()
    {
        mission = GameObject.Find("Network Game Manager")?.GetComponent<Mission>();

        text1 = GameObject.Find("resource1Text").GetComponent<TMP_Text>();
        if (Runner.IsServer)
        {
            AddRobotResources(1000);
            AddCellResources(1000);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Runner.LocalPlayer == mission.cellPlayerRef)
        {
            text1.SetText(cellResources.ToString());
        }
        else
        {
            text1.SetText(robotResources.ToString());
        }
    }


    public int GetCellResources()
    {
        return cellResources;
    }

    public int GetRobotResources()
    {
        return robotResources;
    }

    public void AddCellResources(int num)
    {
        RPCAddCellResources(num);
    }

    public bool ReduceCellResources(int num)
    {
        if (cellResources >= num)
        {
            RPCAddCellResources(-num);
            return true;
        }

        return false;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCAddCellResources(int num)
    {
        cellResources += num;
    }

    public void AddRobotResources(int num)
    {
        RPCAddRobotResources(num);
    }

    public bool ReduceRobotResources(int num)
    {
        if (robotResources >= num)
        {
            RPCAddRobotResources(-num);
            return true;
        }

        return false;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCAddRobotResources(int num)
    {
        robotResources += num;
    }
}