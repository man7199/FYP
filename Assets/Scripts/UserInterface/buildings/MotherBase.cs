using Fusion;
using UnityEngine;

public class MotherBase : Building
{
    [Networked] public NetworkPrefabRef prefab1 { get; set; }
    [Networked] public NetworkPrefabRef prefab2 { get; set; }
    int[] requireresource1 = { 100, 100, 250, 250, 500, 0 };
    int[] requiretime1 = { 3, 3, 0, 0, 0, 3 };
    private GameLevel gameLevel;
    private UnitUI UI;
    private BuildingUI BuildUI;
    bool[] Check = { false, false, false };
    // Start is called before the first frame update
    void Start()
    {
        hp = 500;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        name = "Bone Marrow(Base)";
        gameLevel = GameLevel.Instance;

        description[0] = "Produce a red blood cell";
        sprites[0] = Resources.Load<Sprite>("Icons/Cell/redbloodcell");

        description[1] = "Produce a platele";
        sprites[1] = Resources.Load<Sprite>("Icons/Cell/platelet");

        description[2] = "Unlock Thymus";
        sprites[2] = Resources.Load<Sprite>("Arts/UI/Building/thymus");

        description[3] = "Unlock marrow II";
        sprites[3] = Resources.Load<Sprite>("Arts/UI/Building/bonemarrow2");

        description[4] = "Unlock cell wall";
        sprites[4] = Resources.Load<Sprite>("Arts/UI/Building/cellWall");

        description[5] = null;
        icon = Resources.Load<Sprite>("Arts/UI/Building/bonemarrow"); 
        UI = GameObject.Find("UnitUI").GetComponent<UnitUI>();
        BuildUI = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
    }

        private string Info
    = "This is Mother Base(Bone Marrow), which is an very important building. You lose the game if this building is destroyed. It produces red blood cell, platele, and unlock thymus and marrow II.";
    public override string getInfo()
    {
        return Info;
    }
    public override void Effect1()
    {
        RpcSpawnUnit(prefab1, Runner.LocalPlayer);
    }

    public override void Effect2()
    {
        RpcSpawnUnit(prefab2, Runner.LocalPlayer);
    }

    public override void Effect3()
    {
        if (!Check[0])
        {
            Check[0] = true;
            description[2] = null;
            Buildingprogress(5, 2);
            BuildUI.Refresh(this);
        }
        else
        {
            RpcUnlockThymus();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Platelet");
            foreach (GameObject obj in objects)
            {
                if (obj.GetComponent<Platelet>() != null)
                {
                    obj.GetComponent<Platelet>().Unlock();
                }
            }
            if (UI.selected() != null)
            {
                if (UI.selected().GetComponent<Platelet>() != null)
                    UI.Refresh(UI.selected());
            }
        }
    }

    public override void Effect4()
    {
    if (!Check[1])
    {
            Check[1] = true;
            description[3] = null;
        Buildingprogress(5, 3);
        BuildUI.Refresh(this);
    }
    else
    {
            RpcUnlockMarrow2();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("AllyProducer");
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<BoneMarrow1>() != null)
            {
                obj.GetComponent<BoneMarrow1>().Unlock();
            }
        }
        if (BuildUI.selected() != null)
        {
            if (BuildUI.selected().GetComponent<BoneMarrow1>() != null)
                BuildUI.Refresh(BuildUI.selected());
        }
    }
    }

    public override void Effect5()
    {
        if (!Check[2])
        {
            Check[2] = true;
            description[4] = null;
            Buildingprogress(5, 4);
            BuildUI.Refresh(this);
        }
        else
        {
            RpcUnlockCellWall();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Platelet");
            foreach (GameObject obj in objects)
            {
                if (obj.GetComponent<Platelet>() != null)
                {
                    obj.GetComponent<Platelet>().Unlock();
                }
            }
            if (UI.selected() != null)
            {
                if (UI.selected().GetComponent<Platelet>() != null)
                    UI.Refresh(UI.selected());
            }
        }
    }
    

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcSpawnUnit(NetworkPrefabRef prefabRef, PlayerRef playerRef)
    {
        Vector3 position = new Vector3(transform.position.x + 5 + Random.value * 3,
            transform.position.y, transform.position.z - 5 + Random.value * 3);
        NetworkObject newObject = Runner.Spawn(prefabRef, position, Quaternion.identity);
        Unit unit = newObject.GetComponent<Unit>();
        unit.Owner = playerRef;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcUnlockThymus()
    {
        gameLevel.isThymusUnlock = true;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcUnlockMarrow2()
    {
        gameLevel.isMarrow2Unlock = true;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcUnlockCellWall()
    {
        gameLevel.isCellWallUnlock = true;
    }
    

}